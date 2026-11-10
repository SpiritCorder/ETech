using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("Specialisation")]
public class Specialisation
{
    [Key]
    public int SpecialisationID { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "Specialisation Name")]
    public string SpecialisationName { get; set; } = string.Empty;
}
