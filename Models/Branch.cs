using System.ComponentModel.DataAnnotations;

namespace SunRiseCo.Models
{
    public class Branch
    {
        public int Id { get; set; }
        [MinLength(5, ErrorMessage = "Name must not less than 5 chars")]
        [MaxLength(50, ErrorMessage = "Name must not more than 50 chars")]

        public string Name { get; set; }

        public string Address { get; set; }
    }
}
