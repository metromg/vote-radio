using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radio.Core.Domain.Playback.Model;
using Radio.Infrastructure.DbAccess.Extensions;

namespace Radio.Infrastructure.DbAccess.Mapping.Playback
{
    public class CurrentSongMapping : IEntityTypeConfiguration<CurrentSong>
    {
        public void Configure(EntityTypeBuilder<CurrentSong> builder)
        {
            builder.ConfigureEntityBaseProperties();

            builder.HasOne(e => e.Song).WithMany().HasForeignKey(e => e.SongId);
        }
    }
}
