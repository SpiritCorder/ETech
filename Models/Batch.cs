using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("Batch")]
public class Batch
{
    [Key]
    public int BatchID { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Batch Name")]
    public string BatchName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Course")]
    public int CourseID { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    [ForeignKey("CourseID")]
    public Course? Course { get; set; }
}
