using API_Training3.App.Databases.MongoDB;
using API_Training3.App.Helper;
using API_Training3.App.Services;
using API_Training3.Modules.CostTypes.Entities;
using API_Training3.Modules.CostTypes.Requests;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic.Core;

using System.Threading.Tasks;

namespace API_Training3.Modules.CostTypes.Services
{
    public interface ICostTypeServices
    {
        CostType Store(CostTypeRequest costTypeRequest);
        List<CostType> GetAll();
        SearchResponse Search(SearchRequest request);
        (Object data, string message) Update(CostTypeRequest costTypeRequest, string id);
        (Object data, string message) Delete(string ID);

    }

    public class CostTypeServices : ICostTypeServices
    {
        protected readonly IConfiguration configuration;
        protected readonly IMongoDBWrapper _mongoDB;
        protected readonly IMapper mapper;

        public CostTypeServices(IConfiguration iConfig, IMongoDBWrapper mongoDBWrapper, IMapper map)
        {
            configuration = iConfig;
            _mongoDB = mongoDBWrapper;
            mapper = map;
        }

        public (object data, string message) Delete(string ID)
        {
            CostType costType = _mongoDB.CostTypes.FindById(ID);
            if (costType == null)
            {
                return (null, "This cost type id is not exist");
            }
            return (costType, "Cost type id: " + costType.Id + " removed");
        }

        public List<CostType> GetAll()
        {
            return _mongoDB.CostTypes.GetAll().ToList();
        }

        public SearchResponse Search(SearchRequest request)
        {
            var querySearch = _mongoDB.CostTypes.FindByCondition(c => c.CostTypeName.Contains(request.Search, StringComparison.OrdinalIgnoreCase));
            if (request.SortField != null)
            {
                string orderBy = "asc";
                if (request.SortOrder == SortOrder.Descending)
                {
                    orderBy = "desc";
                }
                querySearch = querySearch.OrderBy(request.SortField + " " + orderBy);
            }

            SearchResponse response = new SearchResponse();
            response.Info.TotalRecord = querySearch.Count();
            if (request.Page != 0)
            {
                response.Data = querySearch.Skip((request.Page - 1) * request.Limit).Take(request.Limit);
                response.Info.Limit = request.Limit;
            }
            else
            {
                response.Info.Limit = querySearch.Count();
                response.Data = querySearch.ToList();
            }
            return response;
        }

        public CostType Store(CostTypeRequest costTypeRequest)
        {
            CostType costType = mapper.Map<CostType>(costTypeRequest);
            _mongoDB.CostTypes.Add(costType);
            return costType;
        }

        public (object data, string message) Update(CostTypeRequest costTypeRequest, string id)
        {
            CostType costTypes = _mongoDB.CostTypes.FindById(id);
            if (costTypes != null)
            {
                costTypes = costTypeRequest.MergeData(costTypes);
                _mongoDB.CostTypes.Update(costTypes);
                return (costTypes, "Update success");
            }
            return (null, "Cost type id is not exist");
        }
    }
}