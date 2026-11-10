using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Departments;

public class IndexModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public IndexModel(ETechCollegeContext context)
    {
        _context = context;
    }

    public List<Department> Departments { get; set; } = new();

    public async Task OnGetAsync()
    {
        Departments = await _context.Departments
            .OrderBy(d => d.DepartmentName)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var dept = await _context.Departments.FindAsync(id);
        if (dept == null)
        {
            TempData["Error"] = "Department not found.";
            return RedirectToPage();
        }

        bool hasCourses = await _context.Courses.AnyAsync(c => c.DepartmentID == id);
        bool hasLecturers = await _context.Lecturers.AnyAsync(l => l.DepartmentID == id);
        bool hasExams = await _context.Exams.AnyAsync(e => e.DepartmentID == id);
        if (hasCourses || hasLecturers || hasExams)
        {
            TempData["Error"] = $"Cannot delete \"{dept.DepartmentName}\" because it has associated courses, lecturers, or exams.";
            return RedirectToPage();
        }

        _context.Departments.Remove(dept);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Department \"{dept.DepartmentName}\" deleted successfully.";
        return RedirectToPage();
    }
}
