using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.Students;

public class EditModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public EditModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Student Student { get; set; } = new();

    public SelectList CourseList { get; set; } = null!;
    public SelectList BatchList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return RedirectToPage("/Students/Index");

        Student = student;
        await PopulateDropdowns();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns();
            return Page();
        }

        var existing = await _context.Students.FindAsync(Student.StudentID);
        if (existing == null)
            return RedirectToPage("/Students/Index");

        existing.StudentName = Student.StudentName;
        existing.DateOfBirth = Student.DateOfBirth;
        existing.Gender = Student.Gender;
        existing.NIC = Student.NIC;
        existing.Address = Student.Address;
        existing.ContactNumber = Student.ContactNumber;
        existing.RegisteredDate = Student.RegisteredDate;
        existing.StudentType = Student.StudentType;
        existing.CourseID = Student.CourseID;
        existing.BatchID = Student.BatchID;

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Student \"{Student.StudentName}\" updated successfully.";
        return RedirectToPage("/Students/Index");
    }

    private async Task PopulateDropdowns()
    {
        CourseList = new SelectList(await _context.Courses.ToListAsync(), "CourseID", "CourseName");
        BatchList = new SelectList(await _context.Batches.ToListAsync(), "BatchID", "BatchName");
    }
}
