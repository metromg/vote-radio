using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Radio.Core.Domain.Voting;
using Radio.Infrastructure.DbAccess.Extensions;

namespace Radio.Infrastructure.DbAccess.Mapping.Voting
{
    public class VoteMapping : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.ConfigureEntityBaseProperties();

            builder.HasOne(e => e.VotingCandidate).WithMany().HasForeignKey(e => e.VotingCandidateId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
