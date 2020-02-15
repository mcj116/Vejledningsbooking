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
    [Route("api/students")]
    [Produces("application/json")]
    public class StudentsController : ControllerBase
    {
        private readonly IVejledningsbookingRepository _vejledningsbookingRepository;
        private readonly IMapper _mapper;
        public StudentsController(IVejledningsbookingRepository vejledningsbookingRepository, 
            IMapper mapper)
        {
            _vejledningsbookingRepository = vejledningsbookingRepository ??
                throw new ArgumentNullException(nameof(vejledningsbookingRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        /// <summary>
        /// Get all Students
        /// </summary>
        /// <returns></returns>

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<StudentDto>> GetStudents()
        {
            var studentsFromRepo = _vejledningsbookingRepository.GetStudents();
           

            return Ok(_mapper.Map<IEnumerable<StudentDto>>(studentsFromRepo));
        }

        /// <summary>
        /// Get a single Student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("{studentId}", Name ="GetStudent")]
        public ActionResult<StudentDto> GetStudent(Guid studentId)
        {
            var studentFromRepo = _vejledningsbookingRepository.GetStudent(studentId);

            if (studentFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<StudentDto>(studentFromRepo));
        }

        /// <summary>
        /// Create a Student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<StudentDto> CreateStudent(StudentForCreationDto student)
        {

            var studentEntity = _mapper.Map<Student>(student);

            _vejledningsbookingRepository.AddStudents(studentEntity);
            _vejledningsbookingRepository.Save();

            var studentToReturn = _mapper.Map<StudentDto>(studentEntity);

            return CreatedAtRoute("GetStudent",
                new { studentId = studentToReturn.Id },
                studentToReturn);

        }
        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        /// <summary>
        /// Delete a Student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpDelete("{studentId}")]
        public ActionResult DeleteStudent(Guid studentId)
        {
            var studentFromRepo = _vejledningsbookingRepository.GetStudent(studentId);

            if (studentFromRepo == null)
            {
                return NotFound();
            }

            _vejledningsbookingRepository.DeleteStudent(studentFromRepo);

            _vejledningsbookingRepository.Save();

            return NoContent();
        }
    }
}
