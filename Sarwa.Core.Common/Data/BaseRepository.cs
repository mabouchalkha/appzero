using Sarwa.Core.Common.Contracts;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace Sarwa.Core.Common.Data
{
    public abstract class BaseRepository<TEntity, UContext, TKey> : IBaseRepository<TEntity, UContext, TKey>
        where TEntity : class, IIdentifiableEntity<TKey>, new()
        where UContext : DbContext, new()
    {
        protected abstract Expression<Func<TEntity, bool>> IdentifierPredicate(UContext entityContext, TKey id);
        protected abstract DbSet<TEntity> DbSet(UContext entityContext);

        private IQueryable<TEntity> GetEntities(UContext entityContext, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet(entityContext).AsNoTracking();

            return includeProperties
                        .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        private TEntity GetEntity(UContext entityContext, TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetEntities(entityContext, includeProperties).SingleOrDefault(IdentifierPredicate(entityContext, id));
        }

        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (UContext entityContext = new UContext())
                return GetEntities(entityContext, includeProperties).ToList();
           // DbFunctions.
        }

        public TEntity GetById(TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (UContext entityContext = new UContext())
                return GetEntity(entityContext, id, includeProperties);
        }

        public IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (UContext entityContext = new UContext())
                return GetEntities(entityContext, includeProperties).Where(predicate).ToList();
        }

        public TEntity Add(TEntity entity)
        {
            using (UContext entityContext = new UContext())
            {
                TEntity addedEntity = DbSet(entityContext).Add(entity);

                entityContext.SaveChanges();

                return addedEntity;
            }
        }

        public virtual TEntity Update(TEntity entity)
        {
            using (UContext entityContext = new UContext())
            {
                TEntity existingEntity = GetEntity(entityContext, entity.EntityId);

                if (existingEntity == null)
                    throw new InvalidOperationException();

                entityContext.Entry(entity).State = EntityState.Modified;

                entityContext.SaveChanges();

                return entity;
            }
        }

        public virtual void Delete(TEntity entity)
        {
            using (UContext entityContext = new UContext())
            {
                entityContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }
   
        public virtual void Delete(TKey id)
        {
            using (UContext entityContext = new UContext())
            {
                TEntity entity = GetEntity(entityContext, id);
                entityContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public virtual void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            using (UContext entityContext = new UContext())
            {
                IQueryable<TEntity> entities = GetEntities(entityContext).Where(predicate);

                foreach (var entity in entities)
                {
                    entityContext.Entry(entity).State = EntityState.Deleted;
                }
                
                entityContext.SaveChanges();
            }
        }

        public TEntity ExecuteSql(string sql, List<DbParameter> parms)
        {
            using (UContext entityContext = new UContext())
            {
                DbDataReader reader = (DbDataReader)(ExecuteSql(entityContext, sql, parms));
                ObjectResult<TEntity> result = ((IObjectContextAdapter)entityContext).ObjectContext.Translate<TEntity>(reader);
                return result.FirstOrDefault();
            }
        }

        public IDataReader ExecuteSql(UContext entityContext, string sql, List<DbParameter> parms)
        {
            DbCommand command = entityContext.Database.Connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            if (parms != null)
                foreach (DbParameter p in parms)
                    command.Parameters.Add(p);

            if (entityContext.Database.Connection.State != ConnectionState.Open)
                entityContext.Database.Connection.Open();

            IDataReader reader = command.ExecuteReader();
            return reader;
        }
    }
}
