using Autofac;

namespace Radio.Core
{
    public interface IRootLifetimeScopeProvider
    {
        ILifetimeScope Get();
    }
}
