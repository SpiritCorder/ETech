using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ETechCollege.Pages.Classrooms;

public class EditModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public EditModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Classroom Classroom { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var classroom = await _context.Classrooms.FindAsync(id);
        if (classroom == null)
            return RedirectToPage("/Classrooms/Index");

        Classroom = classroom;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var existing = await _context.Classrooms.FindAsync(Classroom.ClassroomID);
        if (existing == null)
            return RedirectToPage("/Classrooms/Index");

        existing.ClassroomNumber = Classroom.ClassroomNumber;
        existing.Location = Classroom.Location;
        existing.Capacity = Classroom.Capacity;

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Classroom \"{Classroom.ClassroomNumber}\" updated successfully.";
        return RedirectToPage("/Classrooms/Index");
    }
}
