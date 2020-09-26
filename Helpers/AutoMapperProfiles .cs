using AutoMapper;
using Microsoft.AspNetCore.Identity;
using DocumentGenAPI.Entities;
using DocumentGenAPI.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DocumentGenAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
        
            CreateMap<UserAditionalData, UserAditionalDataDTO>().ReverseMap();
            CreateMap<UserAditionalDTO, UserAditionalData>()
                .ForMember(x => x.Profilepic, options => options.Ignore());
            CreateMap<UserAdionalPatchDTO, UserAditionalData>().ReverseMap();

        }
    }
}