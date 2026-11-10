using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("Lecture")]
public class Lecture
{
    [Key]
    public int LectureID { get; set; }

    [Required]
    [Display(Name = "Batch")]
    public int BatchID { get; set; }

    [Required]
    [Display(Name = "Lecturer")]
    public int LecturerID { get; set; }

    [Required]
    [Display(Name = "Classroom")]
    public int ClassroomID { get; set; }

    [Required]
    [Display(Name = "Module")]
    public int ModuleID { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Lecture Date")]
    public DateTime LectureDate { get; set; }

    [Required]
    [DataType(DataType.Time)]
    [Display(Name = "Start Time")]
    public TimeSpan StartTime { get; set; }

    [Required]
    [DataType(DataType.Time)]
    [Display(Name = "End Time")]
    public TimeSpan EndTime { get; set; }

    [ForeignKey("BatchID")]
    public Batch? Batch { get; set; }

    [ForeignKey("LecturerID")]
    public Lecturer? Lecturer { get; set; }

    [ForeignKey("ClassroomID")]
    public Classroom? Classroom { get; set; }

    [ForeignKey("ModuleID")]
    public Module? Module { get; set; }
}
