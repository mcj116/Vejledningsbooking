using System.ComponentModel.DataAnnotations;
using Vejledningsbooking.API.ValidationAttributes;

namespace Vejledningsbooking.API.Model_Dtos_
{
    public abstract class CourseForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a title.")]
        [MaxLength(100, ErrorMessage = "The title shouldn't have more than 100 characters.")]
        public string Title { get; set; }

    }
}

