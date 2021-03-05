using API_Training3.App.Databases;
using API_Training3.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API_Training3.App.Repository.MongoRepository
{
    public class MongoRepositoryBase<T> : IRepository<T> where T : Document
    {
        private readonly IMongoCollection<T> _collection;
        private readonly MongoDBContext DBContext;

        public MongoRepositoryBase(MongoDBContext dBContext)
        {
            DBContext = dBContext;
            _collection = DBContext.MongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public void Add(T entity)
        {
            _collection.InsertOne(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _collection.InsertMany(entities);
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return _collection.AsQueryable().Any(expression);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _collection.AsQueryable().Where(expression).AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, string sortField, SortOrder sortOrder)
        {
            throw new NotImplementedException();
        }

        public T FindById(string id)
        {
            return _collection.Find(d => d.Id.Equals(id)).FirstOrDefault();
        }

        public IQueryable<T> GetAll()
        {
            return _collection.AsQueryable().AsNoTracking();
        }

        public void Remove(T entity)
        {
            _collection.FindOneAndDelete(s => s.Id.Equals(entity.Id));
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _collection.DeleteMany(ItemWithListOfId(GetListIds(entities)));
        }

        private List<string> GetListIds(IEnumerable<T> entities)
        {

            List<string> listIds = new List<string>();
            entities.ToList().ForEach(e => listIds.Add(e.Id));
            return listIds;
        }
        private FilterDefinition<T> ItemWithListOfId(List<string> ids)
        {
            return Builders<T>.Filter.Where(e => ids.Contains(e.Id));
        }

        public long totalRecord()
        {
            return _collection.CountDocuments(Builders<T>.Filter.Empty);
        }

        private FilterDefinition<T> FilterId(string id)
        {
            return Builders<T>.Filter.Eq("_id", id);
        }
        public void Update(T entity)
        {
            _collection.ReplaceOne(FilterId(entity.Id.ToString()), entity);
        }
    }
}
