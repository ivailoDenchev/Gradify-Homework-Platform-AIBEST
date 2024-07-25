using GradifyWebApplication.Models;
using GradifyWebApplication.Models.Assignments;
using GradifyWebApplication.Models.School;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradifyWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class GradeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GradeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignGrade([FromBody] AssignGradeModel model)
        {
            var teacher = await _userManager.FindByNameAsync(User.Identity.Name);
            if (teacher == null)
            {
                return Unauthorized(new { message = "Teacher not authenticated" });
            }

            var student = await _userManager.FindByIdAsync(model.StudentId);
            if (student == null)
            {
                return NotFound(new { message = "Student not found" });
            }

            var studentGrade = new StudentGrade
            {
                StudentId = model.StudentId,
                Grade = model.Grade,
                Description = model.Description,
                TeacherId = teacher.Id,
                DateAssigned = DateTime.UtcNow
            };

            _context.StudentGrades.Add(studentGrade);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Grade assigned successfully" });
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentGrades(string studentId)
        {
            var grades = await _context.StudentGrades
                .Where(g => g.StudentId == studentId)
                .Include(g => g.Teacher)
                .ToListAsync();

            if (!grades.Any())
            {
                return NotFound(new { message = "No grades found for the student" });
            }

            var result = grades.Select(g => new
            {
                g.Id,
                g.Grade,
                g.Description,
                g.DateAssigned,
                TeacherName = g.Teacher.UserName
            });

            return Ok(result);
        }
    }
}
