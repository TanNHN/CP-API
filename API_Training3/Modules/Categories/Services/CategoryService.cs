using API_Training3.App.Databases.MongoDB;
using API_Training3.App.Helper;
using API_Training3.App.Services;
using API_Training3.Entities;
using API_Training3.Modules.Categories.Requests;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Modules.Categories.Services
{
    public interface ICategoryService : IService<Category>
    {
        Category Store(StoreCategoryRequest request);
        (Category category, string message) Update(UpdateCategoryRequest request, string id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly IMapper Mapper;
        private readonly IMongoDBWrapper _mongoDb;

        public CategoryService(IMapper mapper, IMongoDBWrapper mongoDb)
        {
            Mapper = mapper;
            _mongoDb = mongoDb;
        }

        public List<Category> GetAll()
        {
            return _mongoDb.Categories.GetAll().ToList();

        }

        public (Category data, string message) GetById(string id)
        {
            Category category = _mongoDb.Categories.FindById(id);
            if (category == null)
            {
                return (null, "This category is not exist");
            }
            else
            {
                return (category, "Success");
            }
        }

        public SearchResponse Search(SearchRequest request)
        {
            throw new NotImplementedException();
        }

        public Category Store(Category entity)
        {
            _mongoDb.Categories.Add(entity);
            return entity;
        }

        public Category Store(StoreCategoryRequest request)
        {
            Category category = Mapper.Map<Category>(request);
            return Store(category);
        }

        public (Category category, string message) Update(UpdateCategoryRequest request, string id)
        {
            Category category = _mongoDb.Categories.FindById(id);
            if (category == null)
            {
                return (null, "This category is not exist");
            }
            category = request.MergeData<Category>(category);
            _mongoDb.Categories.Update(category);
            return (category, "Update Success");
        }
    }
}
