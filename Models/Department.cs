using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SunRiseCo.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Da5al Asm el Department ya 8aly Mad7aknash")]
        [MinLength(2, ErrorMessage = "Name Must Not be less than 2 Characters")]
        [MaxLength(20, ErrorMessage = "Name Must not Exceed 20 Characters")]
        public string? DepName { get; set; }

        [Required(ErrorMessage = "You have to provide a valid Description")]
        [MinLength(5, ErrorMessage = "Description must not be less than 5 characters")]
        [MaxLength(50, ErrorMessage = "Description Must not Exceed 50 Characters")]
        public string? Description { get; set; }

        [ValidateNever]
        public ICollection<Employee> Employees { get; set; }
    }
}
