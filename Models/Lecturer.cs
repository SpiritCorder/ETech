using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("Lecturer")]
public class Lecturer
{
    [Key]
    public int LecturerID { get; set; }

    [Required]
    [StringLength(150)]
    [Display(Name = "Lecturer Name")]
    public string LecturerName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [StringLength(20)]
    [Display(Name = "NIC")]
    public string NIC { get; set; } = string.Empty;

    [Required]
    [StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Display(Name = "Contact Number")]
    public string ContactNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Display(Name = "Civil Status")]
    public string CivilStatus { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Gender { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Qualifications { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Department")]
    public int DepartmentID { get; set; }

    [ForeignKey("DepartmentID")]
    public Department? Department { get; set; }
}
