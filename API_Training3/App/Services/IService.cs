using API_Training3.App.Helper;
using API_Training3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.App.Services
{
    public interface IService<T> where T : Document
    {
        T Store(T entity);
        List<T> GetAll();
        (T data, string message) GetById(string id);

        SearchResponse Search(SearchRequest request);
    }
}
