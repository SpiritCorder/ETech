using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Batches;

public class EditModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public EditModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Batch Batch { get; set; } = new();

    public SelectList CourseList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var batch = await _context.Batches.FindAsync(id);
        if (batch == null)
            return RedirectToPage("/Batches/Index");

        Batch = batch;
        CourseList = new SelectList(await _context.Courses.OrderBy(c => c.CourseName).ToListAsync(), "CourseID", "CourseName");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            CourseList = new SelectList(await _context.Courses.OrderBy(c => c.CourseName).ToListAsync(), "CourseID", "CourseName");
            return Page();
        }

        var existing = await _context.Batches.FindAsync(Batch.BatchID);
        if (existing == null)
            return RedirectToPage("/Batches/Index");

        existing.BatchName = Batch.BatchName;
        existing.CourseID = Batch.CourseID;
        existing.StartDate = Batch.StartDate;
        existing.EndDate = Batch.EndDate;

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Batch \"{Batch.BatchName}\" updated successfully.";
        return RedirectToPage("/Batches/Index");
    }
}
