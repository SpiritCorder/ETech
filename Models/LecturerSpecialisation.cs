using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("LecturerSpecialisation")]
public class LecturerSpecialisation
{
    [Required]
    [Display(Name = "Lecturer")]
    public int LecturerID { get; set; }

    [Required]
    [Display(Name = "Specialisation")]
    public int SpecialisationID { get; set; }

    [ForeignKey("LecturerID")]
    public Lecturer? Lecturer { get; set; }

    [ForeignKey("SpecialisationID")]
    public Specialisation? Specialisation { get; set; }
}
