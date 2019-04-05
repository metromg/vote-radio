using Autofac;
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
            builder.RegisterType<CurrentSongService>().As<ICurrentSongService>().InstancePerDependency();
            builder.RegisterType<VotingCandidateService>().As<IVotingCandidateService>().InstancePerDependency();
        }
    }
}
