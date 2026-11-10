using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("Student")]
public class Student
{
    [Key]
    public int StudentID { get; set; }

    [Required]
    [StringLength(150)]
    [Display(Name = "Student Name")]
    public string StudentName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [StringLength(20)]
    public string Gender { get; set; } = string.Empty;

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
    [DataType(DataType.Date)]
    [Display(Name = "Registered Date")]
    public DateTime RegisteredDate { get; set; } = DateTime.Today;

    [Required]
    [StringLength(8)]
    [Display(Name = "Student Type")]
    public string StudentType { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Course")]
    public int CourseID { get; set; }

    [Required]
    [Display(Name = "Batch")]
    public int BatchID { get; set; }

    [ForeignKey("CourseID")]
    public Course? Course { get; set; }

    [ForeignKey("BatchID")]
    public Batch? Batch { get; set; }
}
