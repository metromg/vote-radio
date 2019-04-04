using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radio.Core.Domain.MasterData.Model;
using Radio.Infrastructure.DbAccess.Extensions;

namespace Radio.Infrastructure.DbAccess.Mapping.MasterData
{
    public class SongMapping : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.ConfigureEntityBaseProperties();

            builder.HasOne(e => e.CoverImage).WithMany().HasForeignKey(e => e.CoverImageId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
