using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radio.Core.Domain;
using Radio.Infrastructure.DbAccess.Extensions;

namespace Radio.Infrastructure.DbAccess.Mapping
{
    public class SongMapping : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.ConfigureEntityBaseProperties();
        }
    }
}
