using Microsoft.EntityFrameworkCore;
using Radio.Infrastructure.DbAccess.Mapping.MasterData;
using Radio.Infrastructure.DbAccess.Mapping.Playback;
using Radio.Infrastructure.DbAccess.Mapping.Voting;

namespace Radio.Infrastructure.DbAccess.Configuration
{
    public class ModelCreator : IModelCreator
    {
        public void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("uuid-ossp");

            // MasterData
            builder.ApplyConfiguration(new FileMapping());
            builder.ApplyConfiguration(new ImageMapping());
            builder.ApplyConfiguration(new SongMapping());

            // Playback
            builder.ApplyConfiguration(new CurrentSongMapping());

            // Voting
            builder.ApplyConfiguration(new VoteMapping());
            builder.ApplyConfiguration(new VotingCandidateMapping());
        }
    }
}
