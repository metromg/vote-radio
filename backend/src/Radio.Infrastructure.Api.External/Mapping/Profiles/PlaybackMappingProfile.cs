using Radio.Core.Domain.Playback.Model;
using Radio.Infrastructure.Api.External.Dtos;

namespace Radio.Infrastructure.Api.External.Mapping.Profiles
{
    public class PlaybackMappingProfile : MappingProfileBase
    {
        public PlaybackMappingProfile()
            : base("Playback")
        {
            MapEntitiesToDtos();
            MapDtosToEntities();
        }

        private void MapEntitiesToDtos()
        {
            CreateEntityToDtoMap<CurrentSong, CurrentSongDto>()
                .ForMember(e => e.SongId, opt => opt.MapFrom(s => s.Song.Id))
                .ForMember(e => e.Title, opt => opt.MapFrom(s => s.Song.Title))
                .ForMember(e => e.Album, opt => opt.MapFrom(s => s.Song.Album))
                .ForMember(e => e.Artist, opt => opt.MapFrom(s => s.Song.Artist))
                .ForMember(e => e.CoverImageId, opt => opt.MapFrom(s => s.Song.CoverImageId))
                .ForMember(e => e.VoteCount, opt => opt.MapFrom(s => s.VoteCount))
                .ForMember(e => e.DurationInSeconds, opt => opt.MapFrom(s => s.Song.DurationInSeconds))
                .ForMember(e => e.EndsAtTime, opt => opt.MapFrom(s => s.EndsAtTime.UtcDateTime));
        }

        private void MapDtosToEntities()
        {
        }
    }
}
