using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Specialisations;

public class IndexModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public IndexModel(ETechCollegeContext context)
    {
        _context = context;
    }

    public List<Specialisation> Specialisations { get; set; } = new();

    public async Task OnGetAsync()
    {
        Specialisations = await _context.Specialisations
            .OrderBy(s => s.SpecialisationName)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var spec = await _context.Specialisations.FindAsync(id);
        if (spec == null)
        {
            TempData["Error"] = "Specialisation not found.";
            return RedirectToPage();
        }

        bool inUse = await _context.LecturerSpecialisations.AnyAsync(ls => ls.SpecialisationID == id);
        if (inUse)
        {
            TempData["Error"] = $"Cannot delete \"{spec.SpecialisationName}\" because it is assigned to a lecturer.";
            return RedirectToPage();
        }

        _context.Specialisations.Remove(spec);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Specialisation \"{spec.SpecialisationName}\" deleted successfully.";
        return RedirectToPage();
    }
}
