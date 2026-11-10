using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Modules;

public class IndexModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public IndexModel(ETechCollegeContext context)
    {
        _context = context;
    }

    public List<Module> Modules { get; set; } = new();

    public async Task OnGetAsync()
    {
        Modules = await _context.Modules
            .OrderBy(m => m.ModuleName)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var module = await _context.Modules.FindAsync(id);
        if (module == null)
        {
            TempData["Error"] = "Module not found.";
            return RedirectToPage();
        }

        // Check for dependent records
        bool usedInCourse = await _context.CourseModules.AnyAsync(cm => cm.ModuleID == id);
        bool usedInLecture = await _context.Lectures.AnyAsync(l => l.ModuleID == id);
        if (usedInCourse || usedInLecture)
        {
            TempData["Error"] = $"Cannot delete \"{module.ModuleName}\" because it is assigned to a course or used in a lecture. Remove those associations first.";
            return RedirectToPage();
        }

        _context.Modules.Remove(module);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Module \"{module.ModuleName}\" deleted successfully.";
        return RedirectToPage();
    }
}
