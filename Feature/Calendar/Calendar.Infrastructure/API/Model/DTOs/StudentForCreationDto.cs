using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vejledningsbooking.API.Entities;

namespace Vejledningsbooking.API.Model_Dtos_
{
    public class StudentForCreationDto
    {


        public string StudentFirstName { get; set; }

        public string StudentLastName { get; set; }

        public ICollection<Course> Courses { get; set; }
//= new List<Course>();
    }
}
