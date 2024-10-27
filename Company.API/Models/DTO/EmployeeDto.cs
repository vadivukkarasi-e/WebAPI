using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Company.API.Models.DTO
{
    public class EmployeeDto
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [MaxLength(100, ErrorMessage = "Department cannot exceed 100 characters.")]
        public string Department { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
    }
}
