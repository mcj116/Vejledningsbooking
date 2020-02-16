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

        public int CountStudentBookings(Guid studentId)
        {

            if (studentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(studentId));
            }

            return _context.Bookings.Where(s => s.StudentId == studentId && s.StartDateTime > DateTime.Now).Count();


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
            // return _context.Calendars.ToList();
            return _context.Calendars
                .Include(c => c.Courses)
                .Include(t => t.TimeSlots).ThenInclude(t => t.Teacher)
                .Include(t => t.TimeSlots).ThenInclude(b => b.Bookings)
                .Include(t => t.TimeSlots).ThenInclude(b => b.Bookings).ThenInclude(s => s.Student)
                .ToList();
        }

        public Calendar GetCalendar(Guid calendarId)
        {
            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }
            return _context.Calendars
                .Include(c => c.Courses)
                .Include(t => t.TimeSlots).ThenInclude(t => t.Teacher)
                .Include(t => t.TimeSlots).ThenInclude(b => b.Bookings)
                .Include(t => t.TimeSlots).ThenInclude(b => b.Bookings).ThenInclude(s => s.Student)
                .FirstOrDefault(a => a.Id == calendarId);
        }

        public void AddCalendars(Calendar calendar)
        {
            if (calendar == null)
            {
                throw new ArgumentNullException(nameof(calendar));
            }


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

        public bool TimeSlotOverLap(TimeSlot timeSlot)
        {
            if (timeSlot == null)
            {
                throw new ArgumentNullException(nameof(timeSlot));
            }

            foreach (Teacher teacher in _context.Teachers.Where(t => t.Id == timeSlot.TeacherId).Include(t => t.TimeSlots).ToList())
            {
                foreach (TimeSlot teacherTimeSlot in teacher.TimeSlots)
                {
                    // If new timeslot startdate is within start/end date of existing timeslot = overlap
                    if (teacherTimeSlot.TimeSlotStartDateTime <= timeSlot.TimeSlotStartDateTime && teacherTimeSlot.TimeSlotEndDateTime >= timeSlot.TimeSlotStartDateTime)
                    {
                        return true;
                    }
                    // If new timeslot enddate is within start/end date of existing timeslot = overlap
                    if (teacherTimeSlot.TimeSlotStartDateTime <= timeSlot.TimeSlotEndDateTime && teacherTimeSlot.TimeSlotEndDateTime >= timeSlot.TimeSlotEndDateTime)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public TimeSlot GetTimeSlotForCalendar(Guid timeSlotId, Guid calendarId)
        {
            if (timeSlotId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(timeSlotId));
            }

            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            return _context.TimeSlots
              .Where(c => c.CalendarId == calendarId && c.Id == timeSlotId).FirstOrDefault();
        }

        public void DeleteTimeSlot(TimeSlot timeSlot)
        {
            if (timeSlot == null)
            {
                throw new ArgumentNullException(nameof(timeSlot));
            }

            _context.TimeSlots.Remove(timeSlot);
        }

        public void AddTimeSlot(Guid calendarId, TimeSlot timeSlot)
        {
            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            if (timeSlot == null)
            {
                throw new ArgumentNullException(nameof(timeSlot));
            }
            timeSlot.CalendarId = calendarId;
            _context.TimeSlots.Add(timeSlot);
        }

        public IEnumerable<TimeSlot> GetTimeSlotsForCalendar(Guid calendarId)
        {
            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }


            return _context.TimeSlots
              .Where(c => c.CalendarId == calendarId)
              .Include(t => t.Teacher)
              .Include(b => b.Bookings).ThenInclude(s => s.Student)
                        .OrderBy(c => c.Id).ToList();
        }

        public bool TimeSlotExists(Guid timeSlotId)
        {
            if (timeSlotId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(timeSlotId));
            }

            return _context.TimeSlots.Any(a => a.Id == timeSlotId);
        }

        public IEnumerable<Booking> GetBookingsForTimeSlot(Guid timeSlotId)
        {
            if (timeSlotId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(timeSlotId));
            }

            return _context.Bookings
              .Where(c => c.TimeSlotId == timeSlotId).Include(s =>s.Student).ToList();
        }

        public bool BookingOverLap(Booking booking)
        {
            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking));
            }

            foreach (TimeSlot timeSlot in _context.TimeSlots.Where(t => t.Id == booking.TimeSlotId).Include(t => t.Bookings).ToList())
            {
                foreach (Booking timeslotBooking in timeSlot.Bookings)
                {
                    // If new timeslot startdate is within start/end date of existing timeslot = overlap
                    if (timeslotBooking.StartDateTime <= booking.StartDateTime && timeslotBooking.EndDateTime >= booking.StartDateTime)
                    {
                        return true;
                    }
                    // If new timeslot enddate is within start/end date of existing timeslot = overlap
                    if (timeslotBooking.StartDateTime <= booking.EndDateTime && timeslotBooking.EndDateTime >= booking.EndDateTime)
                    {
                        return true;
                    }
                }
            }

            foreach (Booking studentbooking in _context.Bookings.Where(s=>s.StudentId == booking.StudentId))
            {
                // If new timeslot startdate is within start/end date of existing timeslot = overlap
                if (studentbooking.StartDateTime <= booking.StartDateTime && studentbooking.EndDateTime >= booking.StartDateTime)
                {
                    return true;
                }
                // If new timeslot enddate is within start/end date of existing timeslot = overlap
                if (studentbooking.StartDateTime <= booking.EndDateTime && studentbooking.EndDateTime >= booking.EndDateTime)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddBooking(Guid timeslotId, Booking booking)
        {
            if (timeslotId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(timeslotId));
            }

            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking));
            }
            booking.TimeSlotId = timeslotId;
            _context.Bookings.Add(booking);
        }

        public Booking GetBookingForTimeslot(Guid timeslotId, Guid bookingId)
        {
            if (bookingId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bookingId));
            }


            return _context.Bookings
              .Where(c => c.Id == bookingId).FirstOrDefault();
        }

        public void DeleteBooking(Booking booking)
        {
            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking));
            }

            _context.Bookings.Remove(booking);
        }

        public bool BookingExists(Guid bookingId)
        {
            if (bookingId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bookingId));
            }

            return _context.Bookings.Any(a => a.Id == bookingId);
        }
    }
}
