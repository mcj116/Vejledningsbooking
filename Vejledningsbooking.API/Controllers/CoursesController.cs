using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Vejledningsbooking.API.Model_Dtos_;
using Vejledningsbooking.API.Services;

namespace Vejledningsbooking.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class CoursesController : ControllerBase
    {
        private readonly IVejledningsbookingRepository _vejledningsbookingRepository;
        private readonly IMapper _mapper;
        public CoursesController(IVejledningsbookingRepository vejledningsbookingRepository,
            IMapper mapper)
        {
            _vejledningsbookingRepository = vejledningsbookingRepository ??
                throw new ArgumentNullException(nameof(vejledningsbookingRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get courses for a Teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        [HttpGet("teachers/{teacherId}/courses")]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForTeacher(Guid teacherId)
        {
            if (!_vejledningsbookingRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }

            var coursesForTeacherFromRepo = _vejledningsbookingRepository.GetCoursesForTeacher(teacherId);

            return Ok(_mapper.Map<IEnumerable<CourseDto>>(coursesForTeacherFromRepo));

        }

        /// <summary>
        /// Get a single course for a Teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet("teachers/{teacherId}/courses/{courseId}", Name ="GetCourseForTeacher")]
        public ActionResult<CourseDto> GetCourseForTeacher(Guid teacherId, Guid courseId)
        {
            if (!_vejledningsbookingRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }
            var courseForTeacherFromRepo = _vejledningsbookingRepository.GetCourseForTeacher(teacherId, courseId);

            if (courseForTeacherFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CourseDto>(courseForTeacherFromRepo));
        }

        /// <summary>
        /// Create a course for a Teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost("teachers/{teacherId}/courses")]
        public ActionResult<CourseDto> CreateCourseForTeacher(
            Guid teacherId, CourseForCreationDto course)
        {
            if (!_vejledningsbookingRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }

            var courseEntity = _mapper.Map<Entities.Course>(course);

            _vejledningsbookingRepository.AddCourse(teacherId, courseEntity);
            _vejledningsbookingRepository.Save();

            var courseToReturn = _mapper.Map<CourseDto>(courseEntity);

            return CreatedAtRoute("GetCourseForTeacher",
                new { teacherId = teacherId, courseId = courseToReturn.Id },
                courseToReturn);

        }

        /// <summary>
        /// Update course title for a Teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPut("teachers/{teacherId}/courses/{courseId}")]
        public IActionResult UpdateCourseForTeacher(Guid teacherId, Guid courseId, CourseForUpdateDto course)
        {
            if (!_vejledningsbookingRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }

            var courseForTeacherFromRepo = _vejledningsbookingRepository.GetCourseForTeacher(teacherId, courseId);

            if (courseForTeacherFromRepo == null)
            {
                var courseToAdd = _mapper.Map<Entities.Course>(course);
                courseToAdd.Id = courseId;

                _vejledningsbookingRepository.AddCourse(teacherId, courseToAdd);
                _vejledningsbookingRepository.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForTeacher",
                    new { teacherId = teacherId, courseId = courseToReturn.Id },
                    courseToReturn);

            }
           
            _mapper.Map(course, courseForTeacherFromRepo);

            _vejledningsbookingRepository.UpdateCourse(courseForTeacherFromRepo);
            _vejledningsbookingRepository.Save();

            return NoContent();


        }

        /// <summary>
        /// Patch a course
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("teachers/{teacherId}/courses/{courseId}")]
        public ActionResult PartiallyUpdateCourseForTeacher(Guid teacherId,
    Guid courseId,
    JsonPatchDocument<CourseForUpdateDto> patchDocument)
        {
            if (!_vejledningsbookingRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }

            var courseForTeacherFromRepo = _vejledningsbookingRepository.GetCourseForTeacher(teacherId, courseId);

            if (courseForTeacherFromRepo == null)
            {
                var courseDto = new CourseForUpdateDto();
                patchDocument.ApplyTo(courseDto, ModelState);

                if (!TryValidateModel(courseDto))
                {
                    return ValidationProblem(ModelState);
                }

                var courseToAdd = _mapper.Map<Entities.Course>(courseDto);
                courseToAdd.Id = courseId;

                _vejledningsbookingRepository.AddCourse(teacherId, courseToAdd);
                _vejledningsbookingRepository.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForTeacher",
                    new { teacherId, courseId = courseToReturn.Id },
                    courseToReturn);
            }

            var courseToPatch = _mapper.Map<CourseForUpdateDto>(courseForTeacherFromRepo);
            // add validation
            patchDocument.ApplyTo(courseToPatch, ModelState);

            if (!TryValidateModel(courseToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(courseToPatch, courseForTeacherFromRepo);

            _vejledningsbookingRepository.UpdateCourse(courseForTeacherFromRepo);

            _vejledningsbookingRepository.Save();

            return NoContent();
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpDelete("teachers/{teacherId}/courses/{courseId}")]
        public ActionResult DeleteCourseForTeacher(Guid teacherId, Guid courseId)
        {
            if (!_vejledningsbookingRepository.TeacherExists(teacherId))
            {
                return NotFound();
            }

            var courseForTeacherFromRepo = _vejledningsbookingRepository.GetCourseForTeacher(teacherId, courseId);

            if (courseForTeacherFromRepo == null)
            {
                return NotFound();
            }

            _vejledningsbookingRepository.DeleteCourse(courseForTeacherFromRepo);
            _vejledningsbookingRepository.Save();

            return NoContent();
        }


        /// <summary>
        /// Associate course to a student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpPut("students/{studentId}/courses/{courseId}")]
        public IActionResult UpdateCourseForStudent(Guid studentId, Guid courseId)
        {
            if (!_vejledningsbookingRepository.StudentExists(studentId))
            {
                return NotFound();
            }
            if (!_vejledningsbookingRepository.CourseExists(courseId))
            {
                return NotFound();
            }

            //var courseForStudentFromRepo = _vejledningsbookingRepository.GetCourseForStudent(studentId, courseId);

            //if (courseForStudentFromRepo == null)
            //{
            //    var courseToAdd = _mapper.Map<Entities.Course>(course);
            //    courseToAdd.Id = courseId;

            //    _vejledningsbookingRepository.AddCourse(studentId, courseToAdd);
            //    _vejledningsbookingRepository.Save();

            //    var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

            //    return CreatedAtRoute("GetCourseForTeacher",
            //        new { teacherId = studentId, courseId = courseToReturn.Id },
            //        courseToReturn);

            //}

            _vejledningsbookingRepository.AssociateCourseToStudent(studentId,courseId);
            _vejledningsbookingRepository.Save();

            return NoContent();


        }

        /// <summary>
        /// Associate course to a calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpPut("calendars/{calendarId}/courses/{courseId}")]
        public IActionResult UpdateCourseForCalendar(Guid calendarId, Guid courseId)
        {
            if (!_vejledningsbookingRepository.CalendarExists(calendarId))
            {
                return NotFound();
            }
            if (!_vejledningsbookingRepository.CourseExists(courseId))
            {
                return NotFound();
            }

            //var courseForCalendarFromRepo = _vejledningsbookingRepository.GetCourseForCalendar(calendarId, courseId);

            //if (courseForCalendarFromRepo == null)
            //{
            //    var courseToAdd = _mapper.Map<Entities.Course>(course);
            //    courseToAdd.Id = courseId;

            //    _vejledningsbookingRepository.AddCourse(calendarId, courseToAdd);
            //    _vejledningsbookingRepository.Save();

            //    var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

            //    return CreatedAtRoute("GetCourseForTeacher",
            //        new { teacherId = calendarId, courseId = courseToReturn.Id },
            //        courseToReturn);

            //}

            _vejledningsbookingRepository.AssociateCourseToCalendar(calendarId, courseId);
            _vejledningsbookingRepository.Save();

            return NoContent();


        }
        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

    }
}