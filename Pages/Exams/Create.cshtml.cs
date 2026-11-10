using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Exams;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Exam Exam { get; set; } = new();

    public SelectList DepartmentList { get; set; } = null!;

    public async Task OnGetAsync()
    {
        DepartmentList = new SelectList(await _context.Departments.ToListAsync(), "DepartmentID", "DepartmentName");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            DepartmentList = new SelectList(await _context.Departments.ToListAsync(), "DepartmentID", "DepartmentName");
            return Page();
        }

        _context.Exams.Add(Exam);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Exam \"{Exam.ExamNumber}\" added successfully.";
        return RedirectToPage("/Exams/Index");
    }
}
