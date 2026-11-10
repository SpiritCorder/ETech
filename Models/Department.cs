using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("Department")]
public class Department
{
    [Key]
    public int DepartmentID { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Display(Name = "Contact Number")]
    public string ContactNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(254)]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Location { get; set; } = string.Empty;
}
