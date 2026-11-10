using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Lectures;

public class IndexModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public IndexModel(ETechCollegeContext context)
    {
        _context = context;
    }

    public List<LectureViewModel> Lectures { get; set; } = new();

    public class LectureViewModel
    {
        public int LectureID { get; set; }
        public string BatchName { get; set; } = "";
        public string LecturerName { get; set; } = "";
        public string ClassroomNumber { get; set; } = "";
        public string ModuleName { get; set; } = "";
        public DateTime LectureDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public async Task OnGetAsync()
    {
        var lectures = await _context.Lectures
            .Include(l => l.Batch)
            .Include(l => l.Lecturer)
            .Include(l => l.Classroom)
            .Include(l => l.Module)
            .OrderByDescending(l => l.LectureDate)
            .ThenBy(l => l.StartTime)
            .ToListAsync();

        Lectures = lectures.Select(l => new LectureViewModel
        {
            LectureID = l.LectureID,
            BatchName = l.Batch?.BatchName ?? "—",
            LecturerName = l.Lecturer?.LecturerName ?? "—",
            ClassroomNumber = l.Classroom?.ClassroomNumber ?? "—",
            ModuleName = l.Module?.ModuleName ?? "—",
            LectureDate = l.LectureDate,
            StartTime = l.StartTime,
            EndTime = l.EndTime
        }).ToList();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var lecture = await _context.Lectures.FindAsync(id);
        if (lecture == null)
        {
            TempData["Error"] = "Lecture not found.";
            return RedirectToPage();
        }

        _context.Lectures.Remove(lecture);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Lecture deleted successfully.";
        return RedirectToPage();
    }
}
