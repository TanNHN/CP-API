using API_Training3.App.Repository;
using API_Training3.App.Repository.MongoRepository;
using API_Training3.Entities;
using API_Training3.Modules.Accounts.Entities;
using API_Training3.Modules.CostTypes.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.App.Databases.MongoDB
{
    public interface IMongoDBWrapper
    {
        IRepository<Category> Categories { get; }
        IRepository<Account> Accounts { get; }
        IRepository<CostType> CostTypes { get; }
    }

    public class MongoDBWrapper : IMongoDBWrapper
    {
        public IConfiguration Configuration;
        public MongoDBContext MongoDBContext;
        private MongoRepositoryBase<Category> categories;
        private MongoRepositoryBase<Account> accounts;
        private MongoRepositoryBase<CostType> costTypes;
        public MongoDBWrapper(IConfiguration configuration, MongoDBContext mongoDBContext)
        {
            Configuration = configuration;
            MongoDBContext = mongoDBContext;
        }

     public IRepository<Category> Categories
        {
            get
            {
                return categories ??
                 (categories = new MongoRepositoryBase<Category>(MongoDBContext));
            }
        }
        public IRepository<CostType> CostTypes
        {
            get
            {
                return costTypes ??
                 (costTypes = new MongoRepositoryBase<CostType>(MongoDBContext));
            }
        }

        public IRepository<Account> Accounts
        {
            get
            {
                return accounts ??
                 (accounts = new MongoRepositoryBase<Account>(MongoDBContext));
            }
        }
    }
}
