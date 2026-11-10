using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ETechCollege.Pages.Specialisations;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Specialisation Specialisation { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        _context.Specialisations.Add(Specialisation);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Specialisation \"{Specialisation.SpecialisationName}\" added successfully.";
        return RedirectToPage("/Specialisations/Index");
    }
}
