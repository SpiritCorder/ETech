using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("CourseModule")]
public class CourseModule
{
    [Required]
    [Display(Name = "Course")]
    public int CourseID { get; set; }

    [Required]
    [Display(Name = "Module")]
    public int ModuleID { get; set; }

    [Required]
    [Range(1, 30)]
    [Display(Name = "Module Sequence")]
    public byte ModuleSequence { get; set; }

    [ForeignKey("CourseID")]
    public Course? Course { get; set; }

    [ForeignKey("ModuleID")]
    public Module? Module { get; set; }
}
