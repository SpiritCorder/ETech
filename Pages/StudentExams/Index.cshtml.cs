using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.StudentExams;

public class IndexModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public IndexModel(ETechCollegeContext context)
    {
        _context = context;
    }

    public List<StudentExamViewModel> StudentExams { get; set; } = new();

    public class StudentExamViewModel
    {
        public int StudentID { get; set; }
        public int ExamID { get; set; }
        public string StudentName { get; set; } = "";
        public string ExamDisplay { get; set; } = "";
        public string ResultStatus { get; set; } = "";
        public decimal? Marks { get; set; }
    }

    public async Task OnGetAsync()
    {
        StudentExams = await _context.StudentExams
            .Include(se => se.Student)
            .Include(se => se.Exam)
            .Select(se => new StudentExamViewModel
            {
                StudentID = se.StudentID,
                ExamID = se.ExamID,
                StudentName = se.Student!.StudentName,
                ExamDisplay = se.Exam!.ExamNumber + " - " + se.Exam.Subject,
                ResultStatus = se.ResultStatus,
                Marks = se.Marks
            }).ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int studentId, int examId)
    {
        var studentExam = await _context.StudentExams.FindAsync(studentId, examId);
        if (studentExam == null)
        {
            TempData["Error"] = "Student-Exam registration not found.";
            return RedirectToPage();
        }

        _context.StudentExams.Remove(studentExam);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Student-Exam registration deleted successfully.";
        return RedirectToPage();
    }
}
