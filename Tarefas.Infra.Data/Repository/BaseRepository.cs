﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tarefas.API.Domain.Interfaces;
using Tarefas.API.Infra.Data.Context;

namespace Tarefas.API.Infra.Data.Repository
{
    public class BaseRepository<T> : IBase<T> where T : class
    {
        private readonly DbContextOptions<DBContext> _optionBuilder;
        protected readonly DBContext _dbContext;

        public BaseRepository()
        {
            _optionBuilder = new DbContextOptions<DBContext>();
            _dbContext = new DBContext(_optionBuilder);
        }

        public async Task Add(T entidade)
        {
             await _dbContext.Set<T>().AddAsync(entidade);
             await _dbContext.SaveChangesAsync();
        }

        public async Task Update(T entidade)
        {
            _dbContext.Set<T>().Update(entidade);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T entidade)
        {
            _dbContext.Set<T>().Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }
    }
}
