using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vejledningsbooking.API.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Entities.Course, Model_Dtos_.CourseDto>();
            CreateMap<Model_Dtos_.CourseForCreationDto, Entities.Course>();
            CreateMap<Model_Dtos_.CourseForUpdateDto, Entities.Course>();
        }
    }
}
