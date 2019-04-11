using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Radio.Core;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Services.MasterData;
using File = System.IO.File;
using FileInfo = Radio.Core.Domain.MasterData.Objects.FileInfo;

namespace Radio.Infrastructure.Synchronization.Services
{
    public class SongImportService : ISongImportService
    {
        private readonly ILogger _logger;
        private readonly IClock _clock;
        private readonly IOptions<EnvironmentOptions> _environmentOptions;
        private readonly IUnitOfWorkFactory<ISongRepository, IImageService> _unitOfWorkFactory;

        public SongImportService(ILogger logger, IClock clock, IOptions<EnvironmentOptions> environmentOptions, IUnitOfWorkFactory<ISongRepository, IImageService> unitOfWorkFactory)
        {
            _logger = logger;
            _clock = clock;
            _environmentOptions = environmentOptions;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Import()
        {
            var importDate = _clock.UtcNow;

            _logger.LogInformation("Starting import... Import date: {0}", importDate.ToString());

            AddOrUpdateSongs(importDate);
            RemoveDeadSongs(importDate);

            _logger.LogInformation("Finished import!");
        }

        private void AddOrUpdateSongs(DateTimeOffset importDate)
        {
            foreach (var albumDirectory in Directory.EnumerateDirectories(_environmentOptions.Value.DataDirectoryPath, "*", SearchOption.TopDirectoryOnly))
            {
                using (var unit = _unitOfWorkFactory.Begin())
                {
                    var songCounter = 0;
                    var coverImageFile = GetCoverImageFileOrDefault(albumDirectory);

                    foreach (var songFileName in Directory.EnumerateFiles(albumDirectory, "*.mp3", SearchOption.TopDirectoryOnly))
                    {
                        var song = ImportSong(songFileName, coverImageFile, importDate, unit.Dependent, unit.Dependent2);
                        if (song == null)
                        {
                            continue;
                        }

                        songCounter++;
                    }

                    unit.Commit();

                    _logger.LogInformation("Imported {0} song(s) in album directory {1}.", songCounter, albumDirectory);
                }
            }
        }

        private void RemoveDeadSongs(DateTimeOffset importDate)
        {
            while (true)
            {
                using (var unit = _unitOfWorkFactory.Begin())
                {
                    var nextSongsToRemove = unit.Dependent.GetNextSongsToRemove(importDate, batchSize: 50).ToArray();
                    if (nextSongsToRemove.Length == 0)
                    {
                        break;
                    }

                    foreach (var songToRemove in nextSongsToRemove)
                    {
                        RemoveSong(songToRemove, unit.Dependent, unit.Dependent2);
                    }

                    unit.Commit();

                    _logger.LogInformation("Batch removed {0} dead song(s).", nextSongsToRemove.Length);
                }
            }
        }

        private static FileInfo GetCoverImageFileOrDefault(string albumDirectory)
        {
            var coverImageMimeTypes = new[] { "image/jpeg", "image/png" };
            var coverImageFileName = Directory.EnumerateFiles(albumDirectory, "*", SearchOption.TopDirectoryOnly)
                .FirstOrDefault(fileName => coverImageMimeTypes.Any(mimeType => MimeTypes.GetMimeType(fileName) == mimeType));

            if (string.IsNullOrEmpty(coverImageFileName))
            {
                return null;
            }

            return new FileInfo(MimeTypes.GetMimeType(coverImageFileName), File.ReadAllBytes(coverImageFileName));
        }

        private Song ImportSong(string songFileName, FileInfo coverImageFile, DateTimeOffset importDate, ISongRepository songRepository, IImageService imageService)
        {
            using (var songTagFile = TagLib.File.Create(songFileName))
            {
                var tagValidationResults = ValidateTags(songTagFile);
                if (tagValidationResults.Any())
                {
                    _logger.LogWarning("File {0} is invalid. {1}", songFileName, string.Join(", ", tagValidationResults));
                    return null;
                }

                var songFileInfo = new System.IO.FileInfo(songFileName);
                var relativeSongFileName = Path.Combine(songFileInfo.Directory.Name, songFileInfo.Name);

                var song = songRepository.GetByFileNameOrDefault(relativeSongFileName);
                if (song == null)
                {
                    song = songRepository.Create();
                    songRepository.Add(song);
                }

                song.Map(
                    title: songTagFile.Tag.Title,
                    album: songTagFile.Tag.Album,
                    artist: songTagFile.Tag.FirstPerformer,
                    coverImageFile: coverImageFile,
                    duration: songTagFile.Properties.Duration,
                    fileName: relativeSongFileName,
                    imageService: imageService);

                song.LastImportDate = importDate;

                return song;
            }
        }

        private static void RemoveSong(Song song, ISongRepository songRepository, IImageService imageService)
        {
            song.RemoveCoverImage(imageService);
            songRepository.Remove(song);
        }

        private static IEnumerable<string> ValidateTags(TagLib.File songTagFile)
        {
            if (string.IsNullOrEmpty(songTagFile.Tag.Title))
            {
                yield return "Missing title";
            }

            if (string.IsNullOrEmpty(songTagFile.Tag.Album))
            {
                yield return "Missing album";
            }

            if (string.IsNullOrEmpty(songTagFile.Tag.FirstPerformer))
            {
                yield return "Missing performer";
            }

            if (songTagFile.Properties.Duration.TotalSeconds < Constants.App.MIN_DURATION_IN_SECONDS)
            {
                yield return string.Format("Duration must be greater than {0} seconds.", Constants.App.MIN_DURATION_IN_SECONDS);
            }

            if (songTagFile.PossiblyCorrupt)
            {
                yield return string.Format("File is possibly corrupt. Reasons: {0}", string.Join(", ", songTagFile.CorruptionReasons));
            }
        }
    }
}
