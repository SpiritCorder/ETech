using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Students;

public class IndexModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public IndexModel(ETechCollegeContext context)
    {
        _context = context;
    }

    public List<StudentViewModel> Students { get; set; } = new();

    public class StudentViewModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; } = "";
        public string NIC { get; set; } = "";
        public string StudentType { get; set; } = "";
        public string CourseName { get; set; } = "";
        public string BatchName { get; set; } = "";
    }

    public async Task OnGetAsync()
    {
        Students = await _context.Students
            .Include(s => s.Course)
            .Include(s => s.Batch)
            .OrderBy(s => s.StudentName)
            .Select(s => new StudentViewModel
            {
                StudentID = s.StudentID,
                StudentName = s.StudentName,
                NIC = s.NIC,
                StudentType = s.StudentType,
                CourseName = s.Course!.CourseName,
                BatchName = s.Batch!.BatchName
            }).ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            TempData["Error"] = "Student not found.";
            return RedirectToPage();
        }

        bool hasExams = await _context.StudentExams.AnyAsync(se => se.StudentID == id);
        if (hasExams)
        {
            TempData["Error"] = $"Cannot delete \"{student.StudentName}\" because they have associated exam registrations. Remove those first.";
            return RedirectToPage();
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Student \"{student.StudentName}\" deleted successfully.";
        return RedirectToPage();
    }
}
