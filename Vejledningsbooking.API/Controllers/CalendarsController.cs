using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        [HttpHead]
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
        [HttpGet("{calendarId}", Name ="GetCalendar")]
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

            _vejledningsbookingRepository.AddCalendars(calendarEntity);
            _vejledningsbookingRepository.Save();

            var calendarToReturn = _mapper.Map<CalendarDto>(calendarEntity);

            return CreatedAtRoute("GetCalendar",
                new { calendarId = calendarToReturn.Id },
                calendarToReturn);

        }
        //[HttpOptions]
        //public IActionResult GetAuthorsOptions()
        //{
        //    Response.Headers.Add("Allow", "GET,OPTIONS,POST");
        //    return Ok();
        //}

        /// <summary>
        /// Delete a Calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        [HttpDelete("{calendarId}")]
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
    }
}
