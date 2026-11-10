using ETechCollege.Models;
using Microsoft.EntityFrameworkCore;

namespace ETechCollege.Data;

public class ETechCollegeContext : DbContext
{
    public ETechCollegeContext(DbContextOptions<ETechCollegeContext> options) : base(options) { }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<CourseModule> CourseModules => Set<CourseModule>();
    public DbSet<Batch> Batches => Set<Batch>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Lecturer> Lecturers => Set<Lecturer>();
    public DbSet<Specialisation> Specialisations => Set<Specialisation>();
    public DbSet<LecturerSpecialisation> LecturerSpecialisations => Set<LecturerSpecialisation>();
    public DbSet<Classroom> Classrooms => Set<Classroom>();
    public DbSet<Lecture> Lectures => Set<Lecture>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<StudentExam> StudentExams => Set<StudentExam>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseModule>()
            .HasKey(cm => new { cm.CourseID, cm.ModuleID });

        modelBuilder.Entity<LecturerSpecialisation>()
            .HasKey(ls => new { ls.LecturerID, ls.SpecialisationID });

        modelBuilder.Entity<StudentExam>()
            .HasKey(se => new { se.StudentID, se.ExamID });
    }
}
