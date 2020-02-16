using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vejledningsbooking.API.Profiles
{
    public class TimeSlotsProfile : Profile
    {
        public TimeSlotsProfile()
        {
            CreateMap<Entities.TimeSlot, Model_Dtos_.TimeSlotDto>()
                .ForMember(
                    dest => dest.TeacherName,
                    opt => opt.MapFrom(src => $"{src.Teacher.TeacherFirstName} {src.Teacher.TeacherLastName}"));

            CreateMap<Entities.Booking, Model_Dtos_.TimeSlotDto>();
        }

    }
}
