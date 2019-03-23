using System;
using System.Threading.Tasks;

namespace Radio.Core
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        Task CommitAsync();
    }

    public interface IUnitOfWork<out TDependent> : IUnitOfWork
    {
        TDependent Dependent { get; }
    }

    public interface IUnitOfWork<out TDependent1, out TDependent2> : IUnitOfWork<TDependent1>
    {
        TDependent2 Dependent2 { get; }
    }

    public interface IUnitOfWork<out TDependent1, out TDependent2, out TDependent3> : IUnitOfWork<TDependent1, TDependent2>
    {
        TDependent3 Dependent3 { get; }
    }
}
