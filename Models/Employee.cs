using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunRiseCo.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Da5al Asmak ya 8aly Mad7aknash")]
        [MinLength(4, ErrorMessage = "Name Must Not be less than 4 Characters")]
        [MaxLength(50, ErrorMessage = "Name Must not Exceed 50 Characters")]
        [Display(Name = "Full Name")]
        public string? Name { get; set; }

        [DisplayName("Occupation")]
        [Required(ErrorMessage = "You have to provide a valid position")]
        [MinLength(2, ErrorMessage = "Position must not be less than 2 characters")]
        [MaxLength(20, ErrorMessage = "Position Must not Exceed 20 Characters")]
        public string? Position { get; set; }

        [DataType(DataType.Currency)]
        [Range(2500, 2000000, ErrorMessage = "Salary must be between EGP 2500 and EGP 2M")]
        public double? Salary { get; set; }

        [DataType(DataType.Currency)]
        [Range(1000, double.MaxValue, ErrorMessage = "Bonus must not be less than EGP 1000")]
        public double Bonus { get; set; }

        [RegularExpression("^0\\d{10}$", ErrorMessage = "Invalid Phone Number.")]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone")]
        public string PhoneNo { get; set; }
        [DataType(DataType.EmailAddress)]

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [NotMapped]
        [Compare("Email", ErrorMessage = "Email Not Matched")]
        [DataType(DataType.EmailAddress)]
        public string ConfirmEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Not Match")]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime HiringDateTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [DataType(DataType.Time)]
        public DateTime AttendanceTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime LeavingTime { get; set; }
        [ValidateNever]
        public DateTime CreatedAt { get; set; }
        [ValidateNever]

        public DateTime LastUpdatedAt { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Choose a Valid Department")]
        [DisplayName("Department")]
        public int DepartmentId { get; set; }

        [ValidateNever]
        public Department Department { get; set; }

        [ValidateNever]
        [DisplayName("Image")]
        public string ImageUrl { get; set; }

    }
}