
using System.Runtime;

using AutoMapper;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace DatingAPI.Helpers
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser,MemberDTO>()
            .ForMember(dest => dest.PhotoUrl,
            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age,
            opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            //the above code tells the automapper to map PhotoURL in Member DTO to the main photo url from Photos entity
            //the second ForMember allows us to map age with DOB and extension method CalculateAge()
            CreateMap<Photo,PhotoDTO>();
        }
    }
}