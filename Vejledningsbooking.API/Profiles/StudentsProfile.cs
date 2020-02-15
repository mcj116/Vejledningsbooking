using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vejledningsbooking.API.Profiles
{
    public class StudentsProfile : Profile
    {
        public StudentsProfile()
        {
            CreateMap<Entities.Student, Model_Dtos_.StudentDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.StudentFirstName} {src.StudentLastName}"));

            CreateMap<Model_Dtos_.StudentForCreationDto, Entities.Student>();
        }

    }
}
