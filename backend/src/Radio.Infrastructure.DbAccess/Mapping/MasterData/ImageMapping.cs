using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radio.Core.Domain.MasterData;
using Radio.Infrastructure.DbAccess.Extensions;

namespace Radio.Infrastructure.DbAccess.Mapping.MasterData
{
    public class ImageMapping : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ConfigureEntityBaseProperties();

            builder.HasOne(e => e.File).WithMany().HasForeignKey(e => e.FileId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
