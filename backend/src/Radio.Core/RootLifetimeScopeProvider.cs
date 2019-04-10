using Autofac;

namespace Radio.Core
{
    // Provides access to the root lifetime scope. Should be registerd as SingleInstance.
    public class RootLifetimeScopeProvider : IRootLifetimeScopeProvider
    {
        private readonly ILifetimeScope _rootLifetimeScope;

        public RootLifetimeScopeProvider(ILifetimeScope rootLifetimeScope)
        {
            _rootLifetimeScope = rootLifetimeScope;
        }

        public ILifetimeScope Get()
        {
            return _rootLifetimeScope;
        }
    }
}
