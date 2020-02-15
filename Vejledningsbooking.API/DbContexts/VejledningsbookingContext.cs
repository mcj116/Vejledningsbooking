using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vejledningsbooking.API.Entities;

namespace Vejledningsbooking.API.DbContexts
{/// <summary>
/// Relationships
/// https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
/// </summary>
    public class VejledningsbookingContext : DbContext
    {

        public VejledningsbookingContext(DbContextOptions<VejledningsbookingContext> options)
                   : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId);
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = Guid.Parse("d28111e1-2b29-473a-a40f-e18cb51f9b35"),
                    StudentFirstName = "Erik",
                    StudentLastName = "Sørensen"
                }
                );
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = Guid.Parse("d28888e1-2b29-473a-a40f-e18cb51f9b35"),
                    StudentFirstName = "Hans",
                    StudentLastName = "Andersen"
                }
                );
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher()
                {
                    Id=Guid.Parse("2ee49fe1-edf2-4f21-8409-3eb25ae6ca51"),
                    TeacherFirstName = "Peter",
                    TeacherLastName="Nissen"
                }
                );
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher()
                {
                    Id = Guid.Parse("21e49fe3-e1f2-4f11-8319-3eb15ce1ca51"),
                    TeacherFirstName = "Gunner",
                    TeacherLastName = "Jensen"
                }
                );


            modelBuilder.Entity<Course>().HasData(
                new Course()
                {
                    Id=Guid.Parse("5b1c2b1d-4817-4021-81c3-cc796ad19c6b"),
                    Title= "OPBSW21FD1",
                    TeacherId= Guid.Parse("2ee49fe1-edf2-4f21-8409-3eb25ae6ca51"),
                    StudentId = Guid.Parse("d28111e1-2b29-473a-a40f-e18cb51f9b35") 
                }
                );
            modelBuilder.Entity<Course>()
                .HasData(
                new Course()
                {
                    Id = Guid.Parse("18d63e5e-7194-4f80-8739-610de1bea1ee"),
                    Title = "OPBSW22FD1",
                    TeacherId = Guid.Parse("2ee49fe1-edf2-4f21-8409-3eb25ae6ca51")
                }
                );
            base.OnModelCreating(modelBuilder);
        }

    }
}
