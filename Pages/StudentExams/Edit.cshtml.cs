using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.StudentExams;

public class EditModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public EditModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public StudentExam StudentExam { get; set; } = new();

    public string StudentName { get; set; } = "";
    public string ExamDisplay { get; set; } = "";

    public async Task<IActionResult> OnGetAsync(int studentId, int examId)
    {
        var studentExam = await _context.StudentExams
            .Include(se => se.Student)
            .Include(se => se.Exam)
            .FirstOrDefaultAsync(se => se.StudentID == studentId && se.ExamID == examId);

        if (studentExam == null)
            return RedirectToPage("/StudentExams/Index");

        StudentExam = studentExam;
        StudentName = studentExam.Student?.StudentName ?? "";
        ExamDisplay = studentExam.Exam != null
            ? studentExam.Exam.ExamNumber + " - " + studentExam.Exam.Subject
            : "";

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDisplayFields();
            return Page();
        }

        var existing = await _context.StudentExams.FindAsync(StudentExam.StudentID, StudentExam.ExamID);
        if (existing == null)
            return RedirectToPage("/StudentExams/Index");

        existing.ResultStatus = StudentExam.ResultStatus;
        existing.Marks = StudentExam.Marks;

        await _context.SaveChangesAsync();

        TempData["Success"] = "Exam result updated successfully.";
        return RedirectToPage("/StudentExams/Index");
    }

    private async Task LoadDisplayFields()
    {
        var studentExam = await _context.StudentExams
            .Include(se => se.Student)
            .Include(se => se.Exam)
            .FirstOrDefaultAsync(se => se.StudentID == StudentExam.StudentID && se.ExamID == StudentExam.ExamID);

        if (studentExam != null)
        {
            StudentName = studentExam.Student?.StudentName ?? "";
            ExamDisplay = studentExam.Exam != null
                ? studentExam.Exam.ExamNumber + " - " + studentExam.Exam.Subject
                : "";
        }
    }
}
