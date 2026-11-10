using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Batches;

public class IndexModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public IndexModel(ETechCollegeContext context)
    {
        _context = context;
    }

    public List<BatchViewModel> Batches { get; set; } = new();

    public class BatchViewModel
    {
        public int BatchID { get; set; }
        public string BatchName { get; set; } = "";
        public string CourseName { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public async Task OnGetAsync()
    {
        Batches = await _context.Batches
            .Include(b => b.Course)
            .OrderBy(b => b.StartDate)
            .Select(b => new BatchViewModel
            {
                BatchID = b.BatchID,
                BatchName = b.BatchName,
                CourseName = b.Course!.CourseName,
                StartDate = b.StartDate,
                EndDate = b.EndDate
            }).ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var batch = await _context.Batches.FindAsync(id);
        if (batch == null)
        {
            TempData["Error"] = "Batch not found.";
            return RedirectToPage();
        }

        bool hasStudents = await _context.Students.AnyAsync(s => s.BatchID == id);
        bool hasLectures = await _context.Lectures.AnyAsync(l => l.BatchID == id);
        if (hasStudents || hasLectures)
        {
            TempData["Error"] = $"Cannot delete \"{batch.BatchName}\" because it has associated students or lectures.";
            return RedirectToPage();
        }

        _context.Batches.Remove(batch);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Batch \"{batch.BatchName}\" deleted successfully.";
        return RedirectToPage();
    }
}
