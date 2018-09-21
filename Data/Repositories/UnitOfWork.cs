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

        /// <inheritdoc />
        /// <summary>
        /// Save synchronously any changes synchronously
        /// </summary>
        public void SaveChanges()
        {
            Database.SaveChanges();
        }

        /// <summary>
        /// Save asynchronously any change from the database
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync()
        {
            await Database.SaveChangesAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// Dispose Database Instance
        /// </summary>
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}