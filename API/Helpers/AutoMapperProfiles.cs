using System.Linq;
using API.DTO;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //map from AppUser to MemberDto
            CreateMap<AppUser,MemberDto>()
            .ForMember(dest=>dest.PhotoUrl,opt=>opt.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url))
            .ForMember(dest=>dest.Age,opt=>opt.MapFrom(src=>src.DateOfBirth.CalculateAge())); 
            //map from Photo to PhotoDto
            CreateMap<Photo,PhotoDto>();
        }
    }  
}

