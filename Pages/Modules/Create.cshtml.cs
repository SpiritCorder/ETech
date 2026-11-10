using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ETechCollege.Pages.Modules;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Module Module { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        _context.Modules.Add(Module);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Module \"{Module.ModuleName}\" added successfully.";
        return RedirectToPage("/Modules/Index");
    }
}
