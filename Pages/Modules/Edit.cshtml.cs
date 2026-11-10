using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ETechCollege.Pages.Modules;

public class EditModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public EditModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Module Module { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var module = await _context.Modules.FindAsync(id);
        if (module == null)
            return RedirectToPage("/Modules/Index");

        Module = module;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var existing = await _context.Modules.FindAsync(Module.ModuleID);
        if (existing == null)
            return RedirectToPage("/Modules/Index");

        existing.ModuleName = Module.ModuleName;
        existing.SubjectArea = Module.SubjectArea;

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Module \"{Module.ModuleName}\" updated successfully.";
        return RedirectToPage("/Modules/Index");
    }
}
