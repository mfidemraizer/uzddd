using System;
using System.Threading.Tasks;

namespace UserZoom.Shared.Patterns.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
