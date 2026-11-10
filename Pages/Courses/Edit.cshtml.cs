using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Courses;

public class EditModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public EditModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Course Course { get; set; } = new();

    [BindProperty]
    public List<ModuleAssignment> ModuleAssignments { get; set; } = new();

    public SelectList DepartmentList { get; set; } = null!;

    public class ModuleAssignment
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; } = "";
        public string SubjectArea { get; set; } = "";
        public bool IsSelected { get; set; }
        public byte Sequence { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            return RedirectToPage("/Courses/Index");

        Course = course;
        await LoadFormDataAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadFormDataAsync(Course.CourseID);
            return Page();
        }

        var selected = ModuleAssignments.Where(m => m.IsSelected).ToList();

        if (selected.Any())
        {
            var sequences = selected.Select(m => m.Sequence).ToList();
            if (sequences.Any(s => s < 1 || s > 30))
            {
                ModelState.AddModelError("", "Module sequence must be between 1 and 30.");
                await LoadFormDataAsync(Course.CourseID);
                return Page();
            }
            if (sequences.Distinct().Count() != sequences.Count)
            {
                ModelState.AddModelError("", "Each selected module must have a unique sequence number.");
                await LoadFormDataAsync(Course.CourseID);
                return Page();
            }
        }

        var existing = await _context.Courses.FindAsync(Course.CourseID);
        if (existing == null)
            return RedirectToPage("/Courses/Index");

        existing.CourseName = Course.CourseName;
        existing.DurationMonths = Course.DurationMonths;
        existing.DepartmentID = Course.DepartmentID;

        // Delete old module assignments directly via SQL (bypasses change tracker
        // to avoid key conflicts when re-adding the same CourseID+ModuleID pairs)
        await _context.CourseModules
            .Where(cm => cm.CourseID == Course.CourseID)
            .ExecuteDeleteAsync();

        foreach (var assignment in selected)
        {
            _context.CourseModules.Add(new CourseModule
            {
                CourseID = Course.CourseID,
                ModuleID = assignment.ModuleID,
                ModuleSequence = assignment.Sequence
            });
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Course \"{Course.CourseName}\" updated successfully.";
        return RedirectToPage("/Courses/Index");
    }

    private async Task LoadFormDataAsync(int courseId)
    {
        DepartmentList = new SelectList(
            await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync(),
            "DepartmentID", "DepartmentName");

        if (ModuleAssignments.Count == 0)
        {
            var modules = await _context.Modules.OrderBy(m => m.ModuleName).ToListAsync();
            var currentMappings = await _context.CourseModules
                .Where(cm => cm.CourseID == courseId)
                .ToDictionaryAsync(cm => cm.ModuleID, cm => cm.ModuleSequence);

            ModuleAssignments = modules.Select((m, i) => new ModuleAssignment
            {
                ModuleID = m.ModuleID,
                ModuleName = m.ModuleName,
                SubjectArea = m.SubjectArea ?? "",
                IsSelected = currentMappings.ContainsKey(m.ModuleID),
                Sequence = currentMappings.GetValueOrDefault(m.ModuleID, (byte)(i + 1))
            }).ToList();
        }
        else
        {
            var modules = await _context.Modules.ToDictionaryAsync(m => m.ModuleID);
            foreach (var a in ModuleAssignments)
            {
                if (modules.TryGetValue(a.ModuleID, out var mod))
                {
                    a.ModuleName = mod.ModuleName;
                    a.SubjectArea = mod.SubjectArea ?? "";
                }
            }
        }
    }
}
