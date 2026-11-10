using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Lectures;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Lecture Lecture { get; set; } = new();

    public SelectList BatchList { get; set; } = null!;
    public SelectList LecturerList { get; set; } = null!;
    public SelectList ClassroomList { get; set; } = null!;
    public SelectList ModuleList { get; set; } = null!;

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

        _context.Lectures.Add(Lecture);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Lecture scheduled successfully.";
        return RedirectToPage("/Lectures/Index");
    }

    private async Task PopulateDropdowns()
    {
        BatchList = new SelectList(await _context.Batches.ToListAsync(), "BatchID", "BatchName");
        LecturerList = new SelectList(await _context.Lecturers.ToListAsync(), "LecturerID", "LecturerName");
        ClassroomList = new SelectList(await _context.Classrooms.ToListAsync(), "ClassroomID", "ClassroomNumber");
        ModuleList = new SelectList(await _context.Modules.ToListAsync(), "ModuleID", "ModuleName");
    }
}
