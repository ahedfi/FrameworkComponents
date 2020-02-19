using Ahedfi.Component.Core.Domain.Models.Entities;
using Ahedfi.Component.Core.Domain.Models.Interfaces;
using Ahedfi.Component.Core.Domain.Security.Interfaces;
using Ahedfi.Component.Data.Domain.Interfaces;
using Ahedfi.Component.Services.Domain.Inerfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ahedfi.Component.Services.Domain.Services
{
    public abstract class BaseBusinessServiceProvider<TEntity> : IBusiness<TEntity> where TEntity : Entity, ITransient, IAggregateRoot
    {
        internal protected readonly IMapEngine Mapper;
        private readonly IUnitOfWork _unitOfWork;
        public BaseBusinessServiceProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BaseBusinessServiceProvider(IUnitOfWork unitOfWork, IMapEngine mapper) : this(unitOfWork)
        {
            Mapper = mapper;
        }
        public async Task DeleteAsync(IUserIdentity user, TEntity entity)
        {
            _unitOfWork.Repository<TEntity>().Delete(entity);
           await _unitOfWork.CommitAsync(user.UserName);
        }

        public async Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _unitOfWork.Repository<TEntity>().ListAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await _unitOfWork.Repository<TEntity>().ListAllAsync();
        }

        public async Task<TEntity> FindFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _unitOfWork.Repository<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> SaveAsync(string username, TEntity entity)
        {
            if (entity.IsTransient)
            {
                await _unitOfWork.Repository<TEntity>().AddAsync(entity);
            }
            else
            {
                _unitOfWork.Repository<TEntity>().Update(entity);
            }
            await _unitOfWork.CommitAsync(username);

            return entity;
        }
    }
}
