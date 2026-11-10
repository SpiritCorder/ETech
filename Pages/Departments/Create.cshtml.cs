using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ETechCollege.Pages.Departments;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Department Department { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        _context.Departments.Add(Department);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Department \"{Department.DepartmentName}\" added successfully.";
        return RedirectToPage("/Departments/Index");
    }
}
