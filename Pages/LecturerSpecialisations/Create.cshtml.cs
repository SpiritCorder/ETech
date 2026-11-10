using ETechCollege.Data;
using ETechCollege.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Pages.LecturerSpecialisations;

public class CreateModel : PageModel
{
    private readonly ETechCollegeContext _context;

    public CreateModel(ETechCollegeContext context)
    {
        _context = context;
    }

    [BindProperty]
    public LecturerSpecialisation LecturerSpecialisation { get; set; } = new();

    public SelectList LecturerList { get; set; } = null!;
    public SelectList SpecialisationList { get; set; } = null!;

    public async Task OnGetAsync()
    {
        LecturerList = new SelectList(await _context.Lecturers.ToListAsync(), "LecturerID", "LecturerName");
        SpecialisationList = new SelectList(await _context.Specialisations.ToListAsync(), "SpecialisationID", "SpecialisationName");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            LecturerList = new SelectList(await _context.Lecturers.ToListAsync(), "LecturerID", "LecturerName");
            SpecialisationList = new SelectList(await _context.Specialisations.ToListAsync(), "SpecialisationID", "SpecialisationName");
            return Page();
        }

        _context.LecturerSpecialisations.Add(LecturerSpecialisation);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Lecturer-Specialisation mapping added successfully.";
        return RedirectToPage();
    }
}
