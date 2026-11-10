using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Batches;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Batch Batch { get; set; } = new();

    public SelectList CourseList { get; set; } = null!;

    public async Task OnGetAsync()
    {
        CourseList = new SelectList(await _context.Courses.ToListAsync(), "CourseID", "CourseName");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            CourseList = new SelectList(await _context.Courses.ToListAsync(), "CourseID", "CourseName");
            return Page();
        }

        _context.Batches.Add(Batch);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Batch \"{Batch.BatchName}\" added successfully.";
        return RedirectToPage("/Batches/Index");
    }
}
