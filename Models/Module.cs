using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETechCollege.Models;

[Table("Module")]
public class Module
{
    [Key]
    public int ModuleID { get; set; }

    [Required]
    [StringLength(150)]
    [Display(Name = "Module Name")]
    public string ModuleName { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Subject Area")]
    public string? SubjectArea { get; set; }
}
