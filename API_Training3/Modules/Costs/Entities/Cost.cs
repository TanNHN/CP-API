using API_Training3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Modules.Costs.Entities
{
    public class Cost : Document
    {
        public string CostDescription { get; set; }
        public string CostTypeID { get; set; }
        public double Total { get; set; }
    }
}
