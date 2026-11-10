using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Courses;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
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

    public async Task OnGetAsync()
    {
        await LoadFormDataAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadFormDataAsync();
            return Page();
        }

        var selected = ModuleAssignments.Where(m => m.IsSelected).ToList();

        // Validate sequences for selected modules
        if (selected.Any())
        {
            var sequences = selected.Select(m => m.Sequence).ToList();
            if (sequences.Any(s => s < 1 || s > 30))
            {
                ModelState.AddModelError("", "Module sequence must be between 1 and 30.");
                await LoadFormDataAsync();
                return Page();
            }
            if (sequences.Distinct().Count() != sequences.Count)
            {
                ModelState.AddModelError("", "Each selected module must have a unique sequence number.");
                await LoadFormDataAsync();
                return Page();
            }
        }

        _context.Courses.Add(Course);
        await _context.SaveChangesAsync();

        // Save module assignments
        foreach (var assignment in selected)
        {
            _context.CourseModules.Add(new CourseModule
            {
                CourseID = Course.CourseID,
                ModuleID = assignment.ModuleID,
                ModuleSequence = assignment.Sequence
            });
        }

        if (selected.Any())
            await _context.SaveChangesAsync();

        TempData["Success"] = $"Course \"{Course.CourseName}\" created successfully with {selected.Count} module(s).";
        return RedirectToPage("/Courses/Index");
    }

    private async Task LoadFormDataAsync()
    {
        DepartmentList = new SelectList(
            await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync(),
            "DepartmentID", "DepartmentName");

        // Only reload module list if not a postback (preserve user selections on post)
        if (ModuleAssignments.Count == 0)
        {
            var modules = await _context.Modules.OrderBy(m => m.ModuleName).ToListAsync();
            ModuleAssignments = modules.Select((m, i) => new ModuleAssignment
            {
                ModuleID = m.ModuleID,
                ModuleName = m.ModuleName,
                SubjectArea = m.SubjectArea ?? "",
                IsSelected = false,
                Sequence = (byte)(i + 1)
            }).ToList();
        }
        else
        {
            // Repopulate display names (not posted back)
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
