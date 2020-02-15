using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vejledningsbooking.API.Profiles
{
    public class TeachersProfile : Profile
    {
        public TeachersProfile()
        {
            CreateMap<Entities.Teacher, Model_Dtos_.TeacherDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.TeacherFirstName} {src.TeacherLastName}"));

            CreateMap<Model_Dtos_.TeacherForCreationDto, Entities.Teacher>();
        }

    }
}
