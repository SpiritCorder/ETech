using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Classrooms;

public class IndexModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public IndexModel(ETechCollegeContext context)
    {
        _context = context;
    }

    public List<Classroom> Classrooms { get; set; } = new();

    public async Task OnGetAsync()
    {
        Classrooms = await _context.Classrooms
            .OrderBy(c => c.ClassroomNumber)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var classroom = await _context.Classrooms.FindAsync(id);
        if (classroom == null)
        {
            TempData["Error"] = "Classroom not found.";
            return RedirectToPage();
        }

        bool hasLectures = await _context.Lectures.AnyAsync(l => l.ClassroomID == id);
        if (hasLectures)
        {
            TempData["Error"] = $"Cannot delete \"{classroom.ClassroomNumber}\" because it has associated lectures.";
            return RedirectToPage();
        }

        _context.Classrooms.Remove(classroom);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Classroom \"{classroom.ClassroomNumber}\" deleted successfully.";
        return RedirectToPage();
    }
}
