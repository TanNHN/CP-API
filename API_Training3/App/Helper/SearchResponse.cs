using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.App.Helper
{
    public class SearchResponse
    {
        public Object Data { get; set; }
        public Info Info { get; set; }
    }

    public class Info
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public int TotalRecord { get; set; }
        }
}

