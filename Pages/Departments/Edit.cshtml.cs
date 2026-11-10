using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ETechCollege.Pages.Departments;

public class EditModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public EditModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Department Department { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var dept = await _context.Departments.FindAsync(id);
        if (dept == null)
            return RedirectToPage("/Departments/Index");

        Department = dept;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var existing = await _context.Departments.FindAsync(Department.DepartmentID);
        if (existing == null)
            return RedirectToPage("/Departments/Index");

        existing.DepartmentName = Department.DepartmentName;
        existing.ContactNumber = Department.ContactNumber;
        existing.EmailAddress = Department.EmailAddress;
        existing.Location = Department.Location;

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Department \"{Department.DepartmentName}\" updated successfully.";
        return RedirectToPage("/Departments/Index");
    }
}
