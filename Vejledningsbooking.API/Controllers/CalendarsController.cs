using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vejledningsbooking.API.Entities;
using Vejledningsbooking.API.Model_Dtos_;
using Vejledningsbooking.API.Services;

namespace Vejledningsbooking.API.Controllers
{
    [ApiController]
    [Route("api/calendars")]
    [Produces("application/json")]
    public class CalendarsController : ControllerBase
    {
        private readonly IVejledningsbookingRepository _vejledningsbookingRepository;
        private readonly IMapper _mapper;
        public CalendarsController(IVejledningsbookingRepository vejledningsbookingRepository,
            IMapper mapper)
        {
            _vejledningsbookingRepository = vejledningsbookingRepository ??
                throw new ArgumentNullException(nameof(vejledningsbookingRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        /// <summary>
        /// Get all Calendars
        /// </summary>
        /// <returns></returns>

        [HttpGet()]
     //   [HttpHead]
        public ActionResult<IEnumerable<CalendarDto>> GetCalendars()
        {
            var calendarsFromRepo = _vejledningsbookingRepository.GetCalendars();


            return Ok(_mapper.Map<IEnumerable<CalendarDto>>(calendarsFromRepo));
        }

        /// <summary>
        /// Get a single Calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        [HttpGet("{calendarId}", Name = "GetCalendar")]
        public ActionResult<CalendarDto> GetCalendar(Guid calendarId)
        {
            var calendarFromRepo = _vejledningsbookingRepository.GetCalendar(calendarId);

            if (calendarFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CalendarDto>(calendarFromRepo));
        }

        /// <summary>
        /// Create a Calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<CalendarDto> CreateCalendar(CalendarForCreationDto calendar)
        {

            var calendarEntity = _mapper.Map<Calendar>(calendar);


            foreach (var timeSlot in calendarEntity.TimeSlots)
            {
                if (!_vejledningsbookingRepository.TeacherExists(timeSlot.TeacherId))
                {
                    return NotFound($"Time slot invalid: Noknown Teacher.");
                }

                if (_vejledningsbookingRepository.TimeSlotOverLap(timeSlot))
                {
                    return BadRequest(new ValidationResult($"Timeslot within existing timeslot.", new[] { "Timeslot" }));

                }
                foreach (var booking in timeSlot.Bookings)
                {
                    if (!_vejledningsbookingRepository.StudentExists(booking.StudentId))
                    {
                        return NotFound($"Booking invalid: Noknown Student.");
                    }
                    if (_vejledningsbookingRepository.CountStudentBookings(booking.StudentId) >= 2)
                    {
                        return BadRequest(new ValidationResult($"The maximum booking limit reached.", new[] { "booking.StudentId" }));

                    }
                    if (booking.StartDateTime < timeSlot.TimeSlotStartDateTime || booking.EndDateTime > timeSlot.TimeSlotEndDateTime)
                    {

                        return BadRequest(new ValidationResult($"Booking time range and timeslot time range inconsistency.", new[] { "booking" }));
                    }
                }
            }


            _vejledningsbookingRepository.AddCalendars(calendarEntity);
            _vejledningsbookingRepository.Save();

            var calendarToReturn = _mapper.Map<CalendarDto>(calendarEntity);

            return CreatedAtRoute("GetCalendar",
                new { calendarId = calendarToReturn.Id },
                calendarToReturn);

        }


        /// <summary>
        /// Delete a Calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        [HttpDelete("{calendarId}", Name = "GetCalendar")]
        public ActionResult DeleteCalendar(Guid calendarId)
        {
            var calendarFromRepo = _vejledningsbookingRepository.GetCalendar(calendarId);

            if (calendarFromRepo == null)
            {
                return NotFound();
            }

            _vejledningsbookingRepository.DeleteCalendar(calendarFromRepo);

            _vejledningsbookingRepository.Save();

            return NoContent();
        }

        /// <summary>
        /// Get timeslots for a Calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        [HttpGet("{calendarId}/timeSlots")]
        public ActionResult<IEnumerable<TimeSlotDto>> GetTimeSlotsForCalendar(Guid calendarId)
        {
            if (!_vejledningsbookingRepository.CalendarExists(calendarId))
            {
                return NotFound();
            }

            var timeSlotsForCalendarFromRepo = _vejledningsbookingRepository.GetTimeSlotsForCalendar(calendarId);

            return Ok(_mapper.Map<IEnumerable<TimeSlotDto>>(timeSlotsForCalendarFromRepo));

        }

        /// <summary>
        /// Create a timeslot for a Calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="timeSlot"></param>
        /// <returns></returns>
        [HttpPost("{calendarId}/timeslots")]
        public ActionResult<TimeSlotDto> CreateTimeSlotForCalendar(
            Guid calendarId, TimeSlotForCreationDto timeSlot)
        {
            if (!_vejledningsbookingRepository.TeacherExists(timeSlot.TeacherId))
            {
                return NotFound();
            }

            if (!_vejledningsbookingRepository.CalendarExists(calendarId))
            {
                return NotFound();
            }

            if (!_vejledningsbookingRepository.TeacherExists(timeSlot.TeacherId))
            {
                return NotFound($"Time slot invalid: Noknown Teacher.");
            }



            var timeSlotEntity = _mapper.Map<Entities.TimeSlot>(timeSlot);
            if (_vejledningsbookingRepository.TimeSlotOverLap(timeSlotEntity))
            {
                return BadRequest(new ValidationResult($"Timeslot within existing timeslot, overlap with stored timeslot.", new[] { "Timeslot" }));

            }
            foreach (var booking in timeSlotEntity.Bookings)
            {
                if (!_vejledningsbookingRepository.StudentExists(booking.StudentId))
                {
                    return NotFound($"Booking invalid: Noknown Student.");
                }
                if (_vejledningsbookingRepository.CountStudentBookings(booking.StudentId) >= 2)
                {
                    return BadRequest(new ValidationResult($"The maximum booking limit reached.", new[] { "booking.StudentId" }));

                }
                if (booking.StartDateTime < timeSlot.TimeSlotStartDateTime || booking.EndDateTime > timeSlot.TimeSlotEndDateTime)
                {

                    return BadRequest(new ValidationResult($"Booking time range and timeslot time range inconsistency.", new[] { "booking" }));
                }
            }



            _vejledningsbookingRepository.AddTimeSlot(calendarId, timeSlotEntity);
            _vejledningsbookingRepository.Save();

            var calendarToReturn = _mapper.Map<TimeSlotDto>(timeSlotEntity);

            return CreatedAtRoute("GetCalendar",
                new
                {
                    calendarId = calendarId
                },
                calendarToReturn);

        }
        /// <summary>
        /// Delete a TimeSlot
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="timeSlotId"></param>
        /// <returns></returns>
        [HttpDelete("{calendarId}/timeslots/{timeSlotId}")]
        public ActionResult DeleteTimeSlotForCalendar(Guid calendarId, Guid timeSlotId)
        {
            if (!_vejledningsbookingRepository.CalendarExists(calendarId))
            {
                return NotFound();
            }

            var timeSlotForCalendarFromRepo = _vejledningsbookingRepository.GetTimeSlotForCalendar(timeSlotId, calendarId);

            if (timeSlotForCalendarFromRepo == null)
            {
                return NotFound();
            }

            _vejledningsbookingRepository.DeleteTimeSlot(timeSlotForCalendarFromRepo);
            _vejledningsbookingRepository.Save();

            return NoContent();
        }

        /// <summary>
        /// Get bookings for a TimeSlot
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="timeslotId"></param>
        /// <returns></returns>
        [HttpGet("{calendarId}/timeslots/{timeslotId}/bookings")]
        public ActionResult<IEnumerable<BookingDto>> GetBookingsForTimeSlot(Guid calendarId, Guid timeslotId)
        {
            if (!_vejledningsbookingRepository.TimeSlotExists(timeslotId))
            {
                return NotFound();
            }

            var bookingsForTimeSlotFromRepo = _vejledningsbookingRepository.GetBookingsForTimeSlot(timeslotId);

            return Ok(_mapper.Map<IEnumerable<BookingDto>>(bookingsForTimeSlotFromRepo));

        }

        /// <summary>
        /// Create a booking for a Timeslot
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="timeSlotId"></param>
        /// <param name="booking"></param>
        /// <returns></returns>
        [HttpPost("{calendarId}/timeslots/{timeslotId}/bookings")]
        public ActionResult<BookingDto> CreateBookingForTimeSlot(Guid calendarId,
            Guid timeSlotId, BookingForCreationDto booking)
        {
            if (!_vejledningsbookingRepository.StudentExists(booking.StudentId))
            {
                return NotFound($"Booking invalid: Noknown Student.");
            }

            if (!_vejledningsbookingRepository.TimeSlotExists(timeSlotId))
            {
                return NotFound();
            }

            var bookingEntity = _mapper.Map<Entities.Booking>(booking);


            if (_vejledningsbookingRepository.BookingOverLap(bookingEntity))
            {
                return BadRequest(new ValidationResult($"Booking within existing booking, overlap with stored booking, either student own booking or overlap with other bookings in timeslot.", new[] { "Booking" }));

            }


            if (_vejledningsbookingRepository.CountStudentBookings(booking.StudentId) >= 2)
            {
                return BadRequest(new ValidationResult($"The maximum booking limit reached.", new[] { "booking.StudentId" }));

            }


            _vejledningsbookingRepository.AddBooking(timeSlotId, bookingEntity);
            _vejledningsbookingRepository.Save();

            var calendarToReturn = _mapper.Map<BookingDto>(bookingEntity);

            return CreatedAtRoute("GetCalendar",
                new
                {
                    calendarId = calendarId
                },
                calendarToReturn);

        }
        /// <summary>
        /// Delete a Booking
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="timeSlotId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpDelete("{calendarId}/timeslots/{timeslotId}/bookings/{bookingId}")]
        public ActionResult DeleteBookingForTimeSlot(Guid calendarId, Guid timeSlotId, Guid bookingId)
        {
            if (!_vejledningsbookingRepository.CalendarExists(calendarId))
            {
                return NotFound();
            }

            if (!_vejledningsbookingRepository.TimeSlotExists(timeSlotId))
            {
                return NotFound();
            }

            if (!_vejledningsbookingRepository.BookingExists(bookingId))
            {
                return NotFound();
            }

            var bookingFortimeSlotFromRepo = _vejledningsbookingRepository.GetBookingForTimeslot(timeSlotId, bookingId);

            if (bookingFortimeSlotFromRepo == null)
            {
                return NotFound();
            }

            _vejledningsbookingRepository.DeleteBooking(bookingFortimeSlotFromRepo);
            _vejledningsbookingRepository.Save();

            return NoContent();
        }
    }
}
