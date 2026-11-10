using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.StudentExams;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public StudentExam StudentExam { get; set; } = new();

    public SelectList StudentList { get; set; } = null!;
    public SelectList ExamList { get; set; } = null!;

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

        _context.StudentExams.Add(StudentExam);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Student-Exam registration added successfully.";
        return RedirectToPage("/StudentExams/Index");
    }

    private async Task PopulateDropdowns()
    {
        StudentList = new SelectList(await _context.Students.ToListAsync(), "StudentID", "StudentName");
        ExamList = new SelectList(
            await _context.Exams.Select(e => new { e.ExamID, Display = e.ExamNumber + " - " + e.Subject }).ToListAsync(),
            "ExamID", "Display");
    }
}
