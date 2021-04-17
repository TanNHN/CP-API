using API_Training3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Modules.Accounts.Response
{
    public class DetectResponse
    {
            public int diseaseId { get; set; }
            public List<double> positions { get; set; }
    }  
}
