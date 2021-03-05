using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.App.Databases
{
    public class MongoDBContext : DbContext
    {
        public readonly IMongoDatabase MongoDatabase;
        private readonly IConfiguration Configuration;

        public MongoDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
            MongoClient client = new MongoClient(Configuration["ConnectionSetting:MongoDBSettings:ConnectionStrings"]);
            MongoDatabase = client.GetDatabase(Configuration["ConnectionSetting:MongoDBSettings:DatabaseNames"]);
        }
    }
}
