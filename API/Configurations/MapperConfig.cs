using API.Dto.Auth;
using API.Dto.Catalog;
using API.Entities.Auth;
using API.Entities.Catalog;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {

            CreateMap<Registration, UserDto>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserDto, Registration>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateUserDto, Registration>();

            //Robo model
            CreateMap<RoboModel, ModelDto>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateModelDto, RoboModel>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateModelDto, RoboModel>();
            CreateMap<ModelDto, RoboModel>();
        }
    }
}
