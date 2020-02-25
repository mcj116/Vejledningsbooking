using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vejledningsbooking.API.Entities;
//using Vejledningsbooking.API.Model_Dtos_;

namespace Vejledningsbooking.API.ValidationAttributes
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-3.1
    /// </summary>
    public class BookingMustBeWithinTimeSlotAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
    ValidationContext validationContext)
        {


            return ValidationResult.Success;
        }
    }
}
