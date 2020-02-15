using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vejledningsbooking.API.Entities;
using Vejledningsbooking.API.DbContexts;

namespace Vejledningsbooking.API.Services
{
    public class VejledningsbookingRepository : IVejledningsbookingRepository, IDisposable
    {
        private readonly VejledningsbookingContext _context;

        public VejledningsbookingRepository(VejledningsbookingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddCourse(Guid teacherId, Course course)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            course.TeacherId = teacherId;
            _context.Courses.Add(course);
        }

        public void AddTeachers(Teacher teacher)
        {
            if (teacher == null)
            {
                throw new ArgumentNullException(nameof(teacher));
            }

            // the repository fills the id (instead of using identity columns)
            teacher.Id = Guid.NewGuid();

            foreach (var course in teacher.Courses)
            {
                course.Id = Guid.NewGuid();
            }

            _context.Teachers.Add(teacher);
        }

        public void DeleteCourse(Course course)
        {
            _context.Courses.Remove(course);
        }

        public void DeleteTeacher(Teacher teacher)
        {
            if (teacher == null)
            {
                throw new ArgumentNullException(nameof(teacher));
            }

            _context.Teachers.Remove(teacher);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
        public Course GetCourseForTeacher(Guid teacherId, Guid courseId)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return _context.Courses
              .Where(c => c.TeacherId == teacherId && c.Id == courseId).FirstOrDefault();
        }

        public IEnumerable<Course> GetCoursesForTeacher(Guid teacherId)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            return _context.Courses
                        .Where(c => c.TeacherId == teacherId)
                        .OrderBy(c => c.Title).ToList();
        }

        public IEnumerable<Teacher> GetTeachers()
        {

            return _context.Teachers.Include(t => t.Courses).ToList<Teacher>();
        }

        //public IEnumerable<Teacher> GetTeachers(IEnumerable<Guid> teacherIds)
        //{
        //    if (teacherIds == null)
        //    {
        //        throw new ArgumentNullException(nameof(teacherIds));
        //    }

        //    return _context.Teachers.Where(a => teacherIds.Contains(a.Id))
        //        .OrderBy(a => a.TeacherFirstName)
        //        .OrderBy(a => a.TeacherLastName)
        //        .Include(t => t.Courses)
        //        .ToList();
        //}



        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool TeacherExists(Guid teacherId)
        {
            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            return _context.Teachers.Any(a => a.Id == teacherId);
        }

        public void UpdateCourse(Course course)
        {
            // throw new NotImplementedException();
        }

        public void UpdateTeachers(Teacher teacher)
        {
            throw new NotImplementedException();
        }

        public Teacher GetTeacher(Guid teacherId)
        {

            if (teacherId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teacherId));
            }

            return _context.Teachers.Include(t => t.Courses).FirstOrDefault(a => a.Id == teacherId);

        }

        public IEnumerable<Student> GetStudents()
        {
            return _context.Students.Include(t => t.Courses).ToList<Student>();
        }

        public Student GetStudent(Guid studentId)
        {

            if (studentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(studentId));
            }

            return _context.Students.Include(t => t.Courses).FirstOrDefault(a => a.Id == studentId);

        }

        public void AddStudents(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            // the repository fills the id (instead of using identity columns)
            student.Id = Guid.NewGuid();

            foreach (var course in student.Courses)
            { // TODO: fjdsahf
                course.Id = Guid.NewGuid();
            }

            _context.Students.Add(student);
        }

        public void DeleteStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            _context.Students.Remove(student);
        }

        public bool StudentExists(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(studentId));
            }

            return _context.Students.Any(a => a.Id == studentId);
        }

        public bool CourseExists(Guid courseId)
        {
            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return _context.Courses.Any(a => a.Id == courseId);
        }
        public Course GetCourseForStudent(Guid studentId, Guid courseId)
        {
            if (studentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(studentId));
            }

            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return _context.Courses
              .Where(c => c.StudentId == studentId && c.Id == courseId).FirstOrDefault();

        }

        public Course AssociateCourseToStudent(Guid studentId, Guid courseId)
        {
            if (studentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(studentId));
            }

            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }
            // always set the AuthorId to the passed-in authorId

            _context.Courses.Where(c => c.Id == courseId).FirstOrDefault<Course>().StudentId = studentId;

            return _context.Courses
             .Where(c => c.StudentId == studentId && c.Id == courseId).FirstOrDefault();
        }
        public Course AssociateCourseToCalendar(Guid calendarId, Guid courseId)
        {
            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }
            // always set the AuthorId to the passed-in authorId

            _context.Courses.Where(c => c.Id == courseId).FirstOrDefault<Course>().CalendarId = calendarId;

            return _context.Courses
             .Where(c => c.CalendarId == calendarId && c.Id == courseId).FirstOrDefault();
        }
        public IEnumerable<Calendar> GetCalendars()
        {
            return _context.Calendars.Include(c => c.Courses).Include(t => t.TimeSlots).ThenInclude(b => b.Bookings).ToList();
        }

        public Calendar GetCalendar(Guid calendarId)
        {
            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            return _context.Calendars.Include(t => t.TimeSlots).FirstOrDefault(a => a.Id == calendarId);
        }

        public void AddCalendars(Calendar calendar)
        {
            if (calendar == null)
            {
                throw new ArgumentNullException(nameof(calendar));
            }

            foreach (var timeSlot in calendar.TimeSlots)
            {
                timeSlot.Id = Guid.NewGuid();
            }
            // the repository fills the id (instead of using identity columns)
            // calendar.Id = Guid.NewGuid();

            //foreach (var timeSlot in calendar.TimeSlots)
            //{
            //    timeSlot.Id = Guid.NewGuid();
            //}
            //_context.Students.FirstOrDefault(s => s.Id == calendar.Id).Courses.FirstOrDefault().Calendar.TimeSlots
            _context.Calendars.Add(calendar);
        }

        public void DeleteCalendar(Calendar calendar)
        {
            if (calendar == null)
            {
                throw new ArgumentNullException(nameof(calendar));
            }

            _context.Calendars.Remove(calendar);
        }

        public bool CalendarExists(Guid calendarId)
        {
            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            return _context.Calendars.Any(a => a.Id == calendarId);
        }

        public Course GetCourseForCalendar(Guid calendarId, Guid courseId)
        {
            throw new NotImplementedException();
        }
    }
}
