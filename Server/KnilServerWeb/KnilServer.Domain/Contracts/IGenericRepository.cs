using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KnilServer.Domain.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Expression<Func<T, bool>> condition);
        Task<T> CreateAsync(T contact);
        Task<T> UpdateAsync(T contact);
        Task DeleteAsync(T contact);
        Task SaveAsync();
        bool IsRecordExistsAsync(Expression<Func<T, bool>> condition);
    }
}
