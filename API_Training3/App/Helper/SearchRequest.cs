using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.App.Helper
{
    public class SearchRequest
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public string SortField { get; set; } = "CreateAt";
        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
        public string Search { get; set; } = "";
    }
}
