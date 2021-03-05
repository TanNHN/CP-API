using API_Training3.Entities;
using API_Training3.Modules.Accounts.Entities;
using API_Training3.Modules.Accounts.Requests;
using API_Training3.Modules.Accounts.Response;
using API_Training3.Modules.Categories.Requests;
using API_Training3.Modules.CostTypes.Entities;
using API_Training3.Modules.CostTypes.Requests;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.App
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StoreCategoryRequest, Category>().ForAllMembers(x => x.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<SignUpRequest, Account>().ForAllMembers(x => x.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<LoginRequest, Account>().ForAllMembers(x => x.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<LoginResponse, Account>().ForAllMembers(x => x.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CostTypeRequest, CostType>().ForAllMembers(x => x.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
