using KHCN.Data.Context;
using System;
using System.Threading.Tasks;

namespace KHCN.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync();
        AppDbContext Context { get; }

    }
}
