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
    [Route("api/teachers")]
    [Produces("application/json")]
    public class TeachersController : ControllerBase
    {
        private readonly IVejledningsbookingRepository _vejledningsbookingRepository;
        private readonly IMapper _mapper;
        public TeachersController(IVejledningsbookingRepository vejledningsbookingRepository, 
            IMapper mapper)
        {
            _vejledningsbookingRepository = vejledningsbookingRepository ??
                throw new ArgumentNullException(nameof(vejledningsbookingRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        /// <summary>
        /// Get all Teachers
        /// </summary>
        /// <returns></returns>

        [HttpGet()]
    //    [HttpHead]
        public ActionResult<IEnumerable<TeacherDto>> GetTeachers()
        {
            var teachersFromRepo = _vejledningsbookingRepository.GetTeachers();
           

            return Ok(_mapper.Map<IEnumerable<TeacherDto>>(teachersFromRepo));
        }

        /// <summary>
        /// Get a single Teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        [HttpGet("{teacherId}", Name ="GetTeacher")]
        public ActionResult<TeacherDto> GetTeacher(Guid teacherId)
        {
            var teacherFromRepo = _vejledningsbookingRepository.GetTeacher(teacherId);

            if (teacherFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TeacherDto>(teacherFromRepo));
        }

        /// <summary>
        /// Create a Teacher
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<TeacherDto> CreateTeacher(TeacherForCreationDto teacher)
        {

            var teacherEntity = _mapper.Map<Teacher>(teacher);

            _vejledningsbookingRepository.AddTeachers(teacherEntity);
            _vejledningsbookingRepository.Save();

            var teacherToReturn = _mapper.Map<TeacherDto>(teacherEntity);

            return CreatedAtRoute("GetTeacher",
                new { teacherId = teacherToReturn.Id },
                teacherToReturn);

        }
        //[HttpOptions]
        //public IActionResult GetAuthorsOptions()
        //{
        //    Response.Headers.Add("Allow", "GET,OPTIONS,POST");
        //    return Ok();
        //}

        /// <summary>
        /// Delete a Teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        [HttpDelete("{teacherId}")]
        public ActionResult DeleteTeacher(Guid teacherId)
        {
            var teacherFromRepo = _vejledningsbookingRepository.GetTeacher(teacherId);

            if (teacherFromRepo == null)
            {
                return NotFound();
            }

            _vejledningsbookingRepository.DeleteTeacher(teacherFromRepo);

            _vejledningsbookingRepository.Save();

            return NoContent();
        }
    }
}
