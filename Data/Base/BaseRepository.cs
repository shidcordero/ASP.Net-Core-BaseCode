using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Base
{
    public class BaseRepository
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        protected ApplicationDbContext Context => UnitOfWork.Database;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected virtual DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return Context.Set<TEntity>();
        }

        protected virtual void SetEntityState(object entity, EntityState entityState)
        {
            Context.Entry(entity).State = entityState;
        }
    }
}