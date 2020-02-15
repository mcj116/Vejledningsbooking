using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vejledningsbooking.API.Profiles
{
    public class CalendarsProfile : Profile
    {
        public CalendarsProfile()
        {
            CreateMap<Entities.Calendar, Model_Dtos_.CalendarDto>();
            CreateMap<Model_Dtos_.CalendarForCreationDto, Entities.Calendar>();
        }

    }
}
