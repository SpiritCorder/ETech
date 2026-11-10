using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Courses;

public class IndexModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public IndexModel(ETechCollegeContext context)
    {
        _context = context;
    }

    public List<CourseViewModel> Courses { get; set; } = new();

    public class CourseViewModel
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = "";
        public short DurationMonths { get; set; }
        public string DepartmentName { get; set; } = "";
        public int ModuleCount { get; set; }
    }

    public async Task OnGetAsync()
    {
        await LoadCoursesAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            TempData["Error"] = "Course not found.";
            return RedirectToPage();
        }

        // Check for dependent records
        bool hasStudents = await _context.Students.AnyAsync(s => s.CourseID == id);
        bool hasBatches = await _context.Batches.AnyAsync(b => b.CourseID == id);
        if (hasStudents || hasBatches)
        {
            TempData["Error"] = $"Cannot delete \"{course.CourseName}\" because it has associated students or batches. Remove those first.";
            return RedirectToPage();
        }

        // Remove course-module mappings first
        var mappings = await _context.CourseModules.Where(cm => cm.CourseID == id).ToListAsync();
        _context.CourseModules.RemoveRange(mappings);

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Course \"{course.CourseName}\" deleted successfully.";
        return RedirectToPage();
    }

    private async Task LoadCoursesAsync()
    {
        var courses = await _context.Courses
            .Include(c => c.Department)
            .OrderBy(c => c.CourseName)
            .ToListAsync();

        var moduleCounts = await _context.CourseModules
            .GroupBy(cm => cm.CourseID)
            .Select(g => new { CourseID = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CourseID, x => x.Count);

        Courses = courses.Select(c => new CourseViewModel
        {
            CourseID = c.CourseID,
            CourseName = c.CourseName,
            DurationMonths = c.DurationMonths,
            DepartmentName = c.Department?.DepartmentName ?? "—",
            ModuleCount = moduleCounts.GetValueOrDefault(c.CourseID, 0)
        }).ToList();
    }
}
