using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Lecturers;

public class EditModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public EditModel(ETechCollegeContext context)
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

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var lecturer = await _context.Lecturers.FindAsync(id);
        if (lecturer == null)
            return RedirectToPage("/Lecturers/Index");

        Lecturer = lecturer;
        await LoadFormDataAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadFormDataAsync(Lecturer.LecturerID);
            return Page();
        }

        var existing = await _context.Lecturers.FindAsync(Lecturer.LecturerID);
        if (existing == null)
            return RedirectToPage("/Lecturers/Index");

        existing.LecturerName = Lecturer.LecturerName;
        existing.DateOfBirth = Lecturer.DateOfBirth;
        existing.NIC = Lecturer.NIC;
        existing.Address = Lecturer.Address;
        existing.ContactNumber = Lecturer.ContactNumber;
        existing.CivilStatus = Lecturer.CivilStatus;
        existing.Gender = Lecturer.Gender;
        existing.Qualifications = Lecturer.Qualifications;
        existing.DepartmentID = Lecturer.DepartmentID;

        await _context.LecturerSpecialisations
            .Where(ls => ls.LecturerID == Lecturer.LecturerID)
            .ExecuteDeleteAsync();

        var selected = SpecialisationAssignments.Where(s => s.IsSelected).ToList();
        foreach (var assignment in selected)
        {
            _context.LecturerSpecialisations.Add(new LecturerSpecialisation
            {
                LecturerID = Lecturer.LecturerID,
                SpecialisationID = assignment.SpecialisationID
            });
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Lecturer \"{Lecturer.LecturerName}\" updated successfully.";
        return RedirectToPage("/Lecturers/Index");
    }

    private async Task LoadFormDataAsync(int lecturerId)
    {
        DepartmentList = new SelectList(
            await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync(),
            "DepartmentID", "DepartmentName");

        if (SpecialisationAssignments.Count == 0)
        {
            var specs = await _context.Specialisations.OrderBy(s => s.SpecialisationName).ToListAsync();
            var current = await _context.LecturerSpecialisations
                .Where(ls => ls.LecturerID == lecturerId)
                .Select(ls => ls.SpecialisationID)
                .ToHashSetAsync();

            SpecialisationAssignments = specs.Select(s => new SpecialisationAssignment
            {
                SpecialisationID = s.SpecialisationID,
                SpecialisationName = s.SpecialisationName,
                IsSelected = current.Contains(s.SpecialisationID)
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
