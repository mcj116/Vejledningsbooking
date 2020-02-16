using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vejledningsbooking.API.Entities;

namespace Vejledningsbooking.API.Model_Dtos_
{/// <summary>
/// https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5
/// </summary>
    public class BookingDto
    {

        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
    }
}
