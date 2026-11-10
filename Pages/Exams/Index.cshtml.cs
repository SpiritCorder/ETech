using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Exams;

public class IndexModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public IndexModel(ETechCollegeContext context)
    {
        _context = context;
    }

    public List<ExamViewModel> Exams { get; set; } = new();

    public class ExamViewModel
    {
        public int ExamID { get; set; }
        public string ExamNumber { get; set; } = "";
        public string Subject { get; set; } = "";
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public string Location { get; set; } = "";
        public string DepartmentName { get; set; } = "";
    }

    public async Task OnGetAsync()
    {
        var exams = await _context.Exams
            .Include(e => e.Department)
            .OrderByDescending(e => e.ExamDate)
            .ToListAsync();

        Exams = exams.Select(e => new ExamViewModel
        {
            ExamID = e.ExamID,
            ExamNumber = e.ExamNumber,
            Subject = e.Subject,
            ExamDate = e.ExamDate,
            StartTime = e.StartTime,
            Location = e.Location,
            DepartmentName = e.Department?.DepartmentName ?? "—"
        }).ToList();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam == null)
        {
            TempData["Error"] = "Exam not found.";
            return RedirectToPage();
        }

        bool hasStudentExams = await _context.StudentExams.AnyAsync(se => se.ExamID == id);
        if (hasStudentExams)
        {
            TempData["Error"] = $"Cannot delete \"{exam.ExamNumber}\" because it has associated student exam records. Remove those first.";
            return RedirectToPage();
        }

        _context.Exams.Remove(exam);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Exam \"{exam.ExamNumber}\" deleted successfully.";
        return RedirectToPage();
    }
}
