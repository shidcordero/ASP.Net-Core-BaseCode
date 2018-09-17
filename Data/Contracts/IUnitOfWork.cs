using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface IUnitOfWork
    {
        ApplicationDbContext Database { get; }

        /// <summary>
        /// Saves changes to all objects that have changed within the unit of work.
        /// </summary>
        void SaveChanges();

        Task SaveChangesAsync();
    }
}