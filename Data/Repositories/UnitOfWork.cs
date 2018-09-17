using Data.Contracts;
using System;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public ApplicationDbContext Database { get; private set; }

        public UnitOfWork(ApplicationDbContext serviceContext)
        {
            Database = serviceContext;
        }

        public void SaveChanges()
        {
            Database.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await Database.SaveChangesAsync();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}