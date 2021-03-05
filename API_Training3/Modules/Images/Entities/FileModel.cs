using API_Training3.Entities;
using API_Training3.Modules.Accounts.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Modules.Images.Entities
{
    public class FileModel : Document
    {
        public string accountID { get; set; } 
        public IFormFile MyProperties { get; set; }
    }
}
