using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Lecturers;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Lecturer Lecturer { get; set; } = new();

    [BindProperty]
    public List<SpecialisationAssignment> SpecialisationAssignments { get; set; } = new();

    public SelectList DepartmentList { get; set; } = null!;

    public class SpecialisationAssignment
    {
        public int SpecialisationID { get; set; }
        public string SpecialisationName { get; set; } = "";
        public bool IsSelected { get; set; }
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

        _context.Lecturers.Add(Lecturer);
        await _context.SaveChangesAsync();

        var selected = SpecialisationAssignments.Where(s => s.IsSelected).ToList();
        foreach (var assignment in selected)
        {
            _context.LecturerSpecialisations.Add(new LecturerSpecialisation
            {
                LecturerID = Lecturer.LecturerID,
                SpecialisationID = assignment.SpecialisationID
            });
        }

        if (selected.Any())
            await _context.SaveChangesAsync();

        TempData["Success"] = $"Lecturer \"{Lecturer.LecturerName}\" added successfully with {selected.Count} specialisation(s).";
        return RedirectToPage("/Lecturers/Index");
    }

    private async Task LoadFormDataAsync()
    {
        DepartmentList = new SelectList(
            await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync(),
            "DepartmentID", "DepartmentName");

        if (SpecialisationAssignments.Count == 0)
        {
            var specs = await _context.Specialisations.OrderBy(s => s.SpecialisationName).ToListAsync();
            SpecialisationAssignments = specs.Select(s => new SpecialisationAssignment
            {
                SpecialisationID = s.SpecialisationID,
                SpecialisationName = s.SpecialisationName,
                IsSelected = false
            }).ToList();
        }
        else
        {
            var specs = await _context.Specialisations.ToDictionaryAsync(s => s.SpecialisationID);
            foreach (var a in SpecialisationAssignments)
            {
                if (specs.TryGetValue(a.SpecialisationID, out var spec))
                    a.SpecialisationName = spec.SpecialisationName;
            }
        }
    }
}
