using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("Course")]
public class Course
{
    [Key]
    public int CourseID { get; set; }

    [Required]
    [StringLength(150)]
    [Display(Name = "Course Name")]
    public string CourseName { get; set; } = string.Empty;

    [Required]
    [Range(1, short.MaxValue)]
    [Display(Name = "Duration (Months)")]
    public short DurationMonths { get; set; }

    [Required]
    [Display(Name = "Department")]
    public int DepartmentID { get; set; }

    [ForeignKey("DepartmentID")]
    public Department? Department { get; set; }
}
