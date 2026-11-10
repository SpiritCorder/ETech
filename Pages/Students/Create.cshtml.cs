using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Students;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Student Student { get; set; } = new();

    public SelectList CourseList { get; set; } = null!;
    public SelectList BatchList { get; set; } = null!;

    public async Task OnGetAsync()
    {
        await PopulateDropdowns();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns();
            return Page();
        }

        _context.Students.Add(Student);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Student \"{Student.StudentName}\" registered successfully.";
        return RedirectToPage("/Students/Index");
    }

    private async Task PopulateDropdowns()
    {
        CourseList = new SelectList(await _context.Courses.ToListAsync(), "CourseID", "CourseName");
        BatchList = new SelectList(await _context.Batches.ToListAsync(), "BatchID", "BatchName");
    }
}
