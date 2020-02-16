using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vejledningsbooking.API.Profiles
{
    public class BookingsProfile : Profile
    {
        public BookingsProfile()
        {
            CreateMap<Entities.Booking, Model_Dtos_.BookingDto>()
                .ForMember(
                    dest => dest.StudentName,
                    opt => opt.MapFrom(src => $"{src.Student.StudentFirstName} {src.Student.StudentLastName}"));

        }

    }
}
