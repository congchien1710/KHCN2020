using KHCN.Data.Context;
using KHCN.Data.Interfaces;
using System.Threading.Tasks;

namespace KHCN.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext Context { get; }

        public UnitOfWork(AppDbContext context)
        {
            Context = context;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }
        public async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}