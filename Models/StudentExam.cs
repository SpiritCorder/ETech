using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("StudentExam")]
public class StudentExam
{
    [Required]
    [Display(Name = "Student")]
    public int StudentID { get; set; }

    [Required]
    [Display(Name = "Exam")]
    public int ExamID { get; set; }

    [Required]
    [StringLength(20)]
    [Display(Name = "Result Status")]
    public string ResultStatus { get; set; } = "Registered";

    [Range(0, 100)]
    [Column(TypeName = "decimal(5,2)")]
    public decimal? Marks { get; set; }

    [ForeignKey("StudentID")]
    public Student? Student { get; set; }

    [ForeignKey("ExamID")]
    public Exam? Exam { get; set; }
}
