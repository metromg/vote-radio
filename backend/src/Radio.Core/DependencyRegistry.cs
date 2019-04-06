using Autofac;
using Radio.Core.Services.MasterData;
using Radio.Core.Services.Playback;
using Radio.Core.Services.Voting;

namespace Radio.Core
{
    public static class DependencyRegistry
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<LocalComputerClock>().As<IClock>().InstancePerDependency();

            // Services
            builder.RegisterType<ImageService>().As<IImageService>().InstancePerDependency();
            builder.RegisterType<CurrentSongService>().As<ICurrentSongService>().InstancePerDependency();
            builder.RegisterType<VoteService>().As<IVoteService>().InstancePerDependency();
            builder.RegisterType<VotingCandidateService>().As<IVotingCandidateService>().InstancePerDependency();
        }
    }
}
