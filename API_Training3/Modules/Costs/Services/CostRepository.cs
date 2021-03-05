using API_Training3.App.Repository;
using API_Training3.App.Repository.MongoRepository;
using API_Training3.Entities;
using API_Training3.Modules.Costs.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API_Training3.Modules.Costs.Services
{
    public interface ICostRepository : IRepository<Category>
    {

    }

    public class CostRepository : ICostRepository
    {
        protected readonly MongoRepositoryBase<Cost> _mongodb;

        public CostRepository()
        {

        }

        public void Add(Category entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Category> entities)
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<Category, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Category> FindByCondition(Expression<Func<Category, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Category> FindByCondition(Expression<Func<Category, bool>> expression, string sortField, SortOrder sortOrder)
        {
            throw new NotImplementedException();
        }

        public Category FindById(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Category> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(Category Entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Category> entity)
        {
            throw new NotImplementedException();
        }

        public long totalRecord()
        {
            throw new NotImplementedException();
        }

        public void Update(Category Entity)
        {
            throw new NotImplementedException();
        }
    }
}
