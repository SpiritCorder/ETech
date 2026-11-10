using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("Classroom")]
public class Classroom
{
    [Key]
    public int ClassroomID { get; set; }

    [Required]
    [StringLength(30)]
    [Display(Name = "Classroom Number")]
    public string ClassroomNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [Range(1, short.MaxValue)]
    public short Capacity { get; set; }
}
