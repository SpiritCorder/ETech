using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Exams;

public class EditModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public EditModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Exam Exam { get; set; } = new();

    public SelectList DepartmentList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam == null)
            return RedirectToPage("/Exams/Index");

        Exam = exam;
        DepartmentList = new SelectList(await _context.Departments.ToListAsync(), "DepartmentID", "DepartmentName");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            DepartmentList = new SelectList(await _context.Departments.ToListAsync(), "DepartmentID", "DepartmentName");
            return Page();
        }

        var existing = await _context.Exams.FindAsync(Exam.ExamID);
        if (existing == null)
            return RedirectToPage("/Exams/Index");

        existing.ExamNumber = Exam.ExamNumber;
        existing.Subject = Exam.Subject;
        existing.ExamDate = Exam.ExamDate;
        existing.StartTime = Exam.StartTime;
        existing.Location = Exam.Location;
        existing.DepartmentID = Exam.DepartmentID;

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Exam \"{Exam.ExamNumber}\" updated successfully.";
        return RedirectToPage("/Exams/Index");
    }
}
