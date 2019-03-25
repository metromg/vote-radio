using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radio.Core.Domain.Voting;
using Radio.Infrastructure.DbAccess.Extensions;

namespace Radio.Infrastructure.DbAccess.Mapping.Voting
{
    public class VotingCandidateMapping : IEntityTypeConfiguration<VotingCandidate>
    {
        public void Configure(EntityTypeBuilder<VotingCandidate> builder)
        {
            builder.ConfigureEntityBaseProperties();

            builder.HasOne(e => e.Song).WithMany().HasForeignKey(e => e.SongId);
        }
    }
}
