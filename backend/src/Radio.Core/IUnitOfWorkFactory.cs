namespace Radio.Core
{
    public interface IUnitOfWorkFactory<out T>
    {
        IUnitOfWork<T> Begin();
    }

    public interface IUnitOfWorkFactory<out T1, out T2>
    {
        IUnitOfWork<T1, T2> Begin();
    }

    public interface IUnitOfWorkFactory<out T1, out T2, out T3>
    {
        IUnitOfWork<T1, T2, T3> Begin();
    }
}
