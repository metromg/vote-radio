using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radio.Core.Domain;

namespace Radio.Infrastructure.DbAccess.Extensions
{
    public static class TypeConfigurationExtensions
    {
        public static void ConfigureEntityBaseProperties<T>(this EntityTypeBuilder<T> builder)
            where T : EntityBase
        {
            builder.HasKey(p => p.Id);
        }
    }
}
