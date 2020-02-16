using System;
using System.Collections.Generic;
using Vejledningsbooking.API.Entities;

namespace Vejledningsbooking.API.Services
{
    public interface IVejledningsbookingRepository
    {
        IEnumerable<Course> GetCoursesForTeacher(Guid teacherId);
        Course GetCourseForTeacher(Guid teacherId, Guid courseId);
        void AddCourse(Guid teacherId, Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);
        IEnumerable<Teacher> GetTeachers();
        Teacher GetTeacher(Guid teacherId);
        // TODO: IEnumerable<Teacher> GetTeachers(IEnumerable<Guid> teacherIds);
        void AddTeachers(Teacher teacher);
        void DeleteTeacher(Teacher teacher);
        void UpdateTeachers(Teacher teacher);
        bool TeacherExists(Guid teacherId);
        bool Save();
        IEnumerable<Student> GetStudents();
        Student GetStudent(Guid studentId);
        int CountStudentBookings(Guid studentId);
        void AddStudents(Student student);
        void DeleteStudent(Student student);
        bool StudentExists(Guid studentId);
        Course GetCourseForStudent(Guid studentId, Guid courseId);

        Course AssociateCourseToStudent(Guid studentId, Guid courseId);

        bool CourseExists(Guid courseId);


        IEnumerable<Calendar> GetCalendars();
        Calendar GetCalendar(Guid calendarId);
        void AddCalendars(Calendar calendar);
        void DeleteCalendar(Calendar calendar);
        bool CalendarExists(Guid calendarId);
        Course GetCourseForCalendar(Guid calendarId, Guid courseId);
        Course AssociateCourseToCalendar(Guid calendarId, Guid courseId);
    }
}
