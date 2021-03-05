using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API_Training3.App.Repository
{
    public interface IRepository<T>
    {
        long totalRecord();
        IQueryable<T> GetAll();
        IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, string sortField, SortOrder sortOrder);
        T FindById(string id);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T Entity);
        void Remove(T Entity);
        void RemoveRange(IEnumerable<T> entity);
        bool  Any(Expression<Func<T, bool>> expression);

    }
}
