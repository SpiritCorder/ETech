using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Lecturers;

public class IndexModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public IndexModel(ETechCollegeContext context)
    {
        _context = context;
    }

    public List<LecturerViewModel> Lecturers { get; set; } = new();

    public class LecturerViewModel
    {
        public int LecturerID { get; set; }
        public string LecturerName { get; set; } = "";
        public string NIC { get; set; } = "";
        public string DepartmentName { get; set; } = "";
        public int SpecialisationCount { get; set; }
    }

    public async Task OnGetAsync()
    {
        var lecturers = await _context.Lecturers
            .Include(l => l.Department)
            .OrderBy(l => l.LecturerName)
            .ToListAsync();

        var specCounts = await _context.LecturerSpecialisations
            .GroupBy(ls => ls.LecturerID)
            .Select(g => new { LecturerID = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.LecturerID, x => x.Count);

        Lecturers = lecturers.Select(l => new LecturerViewModel
        {
            LecturerID = l.LecturerID,
            LecturerName = l.LecturerName,
            NIC = l.NIC,
            DepartmentName = l.Department?.DepartmentName ?? "—",
            SpecialisationCount = specCounts.GetValueOrDefault(l.LecturerID, 0)
        }).ToList();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var lecturer = await _context.Lecturers.FindAsync(id);
        if (lecturer == null)
        {
            TempData["Error"] = "Lecturer not found.";
            return RedirectToPage();
        }

        bool hasLectures = await _context.Lectures.AnyAsync(l => l.LecturerID == id);
        if (hasLectures)
        {
            TempData["Error"] = $"Cannot delete \"{lecturer.LecturerName}\" because they have associated lectures.";
            return RedirectToPage();
        }

        await _context.LecturerSpecialisations.Where(ls => ls.LecturerID == id).ExecuteDeleteAsync();
        _context.Lecturers.Remove(lecturer);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Lecturer \"{lecturer.LecturerName}\" deleted successfully.";
        return RedirectToPage();
    }
}
