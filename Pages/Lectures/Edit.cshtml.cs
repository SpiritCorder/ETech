using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Lectures;

public class EditModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public EditModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Lecture Lecture { get; set; } = new();

    public SelectList BatchList { get; set; } = null!;
    public SelectList LecturerList { get; set; } = null!;
    public SelectList ClassroomList { get; set; } = null!;
    public SelectList ModuleList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var lecture = await _context.Lectures.FindAsync(id);
        if (lecture == null)
            return RedirectToPage("/Lectures/Index");

        Lecture = lecture;
        await PopulateDropdowns();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns();
            return Page();
        }

        var existing = await _context.Lectures.FindAsync(Lecture.LectureID);
        if (existing == null)
            return RedirectToPage("/Lectures/Index");

        existing.BatchID = Lecture.BatchID;
        existing.LecturerID = Lecture.LecturerID;
        existing.ClassroomID = Lecture.ClassroomID;
        existing.ModuleID = Lecture.ModuleID;
        existing.LectureDate = Lecture.LectureDate;
        existing.StartTime = Lecture.StartTime;
        existing.EndTime = Lecture.EndTime;

        await _context.SaveChangesAsync();

        TempData["Success"] = "Lecture updated successfully.";
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
