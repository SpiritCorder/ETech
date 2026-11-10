using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ETechCollege.Pages.Specialisations;

public class EditModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public EditModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Specialisation Specialisation { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var spec = await _context.Specialisations.FindAsync(id);
        if (spec == null)
            return RedirectToPage("/Specialisations/Index");

        Specialisation = spec;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var existing = await _context.Specialisations.FindAsync(Specialisation.SpecialisationID);
        if (existing == null)
            return RedirectToPage("/Specialisations/Index");

        existing.SpecialisationName = Specialisation.SpecialisationName;
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Specialisation \"{Specialisation.SpecialisationName}\" updated successfully.";
        return RedirectToPage("/Specialisations/Index");
    }
}
