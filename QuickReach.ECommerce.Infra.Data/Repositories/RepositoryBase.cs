﻿using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly ECommerceDbContext context;
        public RepositoryBase(ECommerceDbContext context)
        {
            this.context = context;
        }
        public TEntity Create(TEntity newEntity)
        {
            this.context.Set<TEntity>()
                .Add(newEntity);
            this.context.SaveChanges();
            return newEntity;
        }

        public void Delete(int entityId)
        {
            var entityToRemove = Retrieve(entityId);
            this.context.Remove<TEntity>(entityToRemove);
            this.context.SaveChanges();
        }

        public TEntity Retrieve(int entityId)
        {
            var entity = this.context.Find<TEntity>(entityId);

            return entity;
        }

        public IEnumerable<TEntity> Retrieve(int skip = 0, int count = 10)
        {
            var result = this.context.Set<TEntity>()
                .Skip(skip)
                .Take(count)
                .ToList();
            return result;
        }

        public TEntity Update(int entityId, TEntity entity)
        {
            var oldEntity = Retrieve(entityId);
            this.context.Update<TEntity>(entity);

            this.context.SaveChanges();

            return entity;
        }
    }
}
