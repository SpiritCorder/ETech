using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.CourseModules;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public CourseModule CourseModule { get; set; } = new();

    public SelectList CourseList { get; set; } = null!;
    public SelectList ModuleList { get; set; } = null!;

    public async Task OnGetAsync()
    {
        CourseList = new SelectList(await _context.Courses.ToListAsync(), "CourseID", "CourseName");
        ModuleList = new SelectList(await _context.Modules.ToListAsync(), "ModuleID", "ModuleName");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            CourseList = new SelectList(await _context.Courses.ToListAsync(), "CourseID", "CourseName");
            ModuleList = new SelectList(await _context.Modules.ToListAsync(), "ModuleID", "ModuleName");
            return Page();
        }

        _context.CourseModules.Add(CourseModule);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Course-Module mapping added successfully.";
        return RedirectToPage();
    }
}
