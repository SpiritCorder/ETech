using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("Exam")]
public class Exam
{
    [Key]
    public int ExamID { get; set; }

    [Required]
    [StringLength(40)]
    [Display(Name = "Exam Number")]
    public string ExamNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Exam Date")]
    public DateTime ExamDate { get; set; }

    [Required]
    [DataType(DataType.Time)]
    [Display(Name = "Start Time")]
    public TimeSpan StartTime { get; set; }

    [Required]
    [StringLength(150)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Department")]
    public int DepartmentID { get; set; }

    [ForeignKey("DepartmentID")]
    public Department? Department { get; set; }
}
