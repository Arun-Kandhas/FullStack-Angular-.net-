using KnilServer.Domain.Contracts;
using KnilServer.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KnilServer.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _Dbcontext;

        public GenericRepository(ApplicationDbContext applicationDbContext)
        {
            _Dbcontext = applicationDbContext;
        }

        public async Task<T> CreateAsync(T contact)
        {
            var AddedEntity =   await _Dbcontext.AddAsync(contact);
            await SaveAsync();
            return AddedEntity.Entity;
        }

        public async Task DeleteAsync(T contact)
        {
            _Dbcontext.Remove(contact);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            var result = await _Dbcontext.Set<T>().AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> condition)
        {
            var contact = await _Dbcontext.Set<T>().AsNoTracking().FirstOrDefaultAsync(condition);
            return contact!;
        }

        public bool IsRecordExistsAsync(Expression<Func<T, bool>> condition)
        {
            var contact = _Dbcontext.Set<T>().AsQueryable().Where(condition).Any();
            return contact;
        }

        public async Task SaveAsync()
        {
            await _Dbcontext.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T contact)
        {
            var UpdatedEntity = _Dbcontext.Set<T>().Update(contact);
            await SaveAsync();
            return UpdatedEntity.Entity;    
        }
    }
}
