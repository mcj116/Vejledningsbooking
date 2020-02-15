using System.ComponentModel.DataAnnotations;
using Vejledningsbooking.API.Model_Dtos_;

namespace Vejledningsbooking.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
    ValidationContext validationContext)
        {
            var course = (CourseForManipulationDto)validationContext.ObjectInstance;

            //if (course.Title == course.Description)
            //{
            //    return new ValidationResult(ErrorMessage,
            //        new[] { nameof(CourseForManipulationDto) });
            //}

            return ValidationResult.Success;
        }
    }
}
