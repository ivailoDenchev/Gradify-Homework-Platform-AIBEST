using GradifyWebApplication.Models.School;
using GradifyWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradifyWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolClassController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SchoolClassController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSchoolClass([FromBody] CreateSchoolClassModel model)
        {
            var teacher = await _userManager.FindByIdAsync(model.TeacherId);
            if (teacher == null)
            {
                return BadRequest("Teacher not found.");
            }

            var schoolClass = new SchoolClass
            {
                Name = model.Name,
                Description = model.Description,
                Teacher = teacher
            };

            _context.SchoolClasses.Add(schoolClass);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Class created successfully" });
        }

        [HttpPost("assign-students")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AssignStudentsToClass([FromBody] AssignStudentsToClassModel model)
        {
            var schoolClass = await _context.SchoolClasses
                .Include(sc => sc.Students)
                .FirstOrDefaultAsync(sc => sc.Id == model.ClassId);

            if (schoolClass == null)
            {
                return BadRequest("Class not found.");
            }

            if (schoolClass.Students == null)
            {
                schoolClass.Students = new List<ApplicationUser>();
            }

            var students = await _userManager.Users
                .Where(u => model.StudentIds.Contains(u.Id))
                .ToListAsync();

            if (!students.Any())
            {
                return BadRequest("No valid students found.");
            }

            foreach (var student in students)
            {
                if (!schoolClass.Students.Any(s => s.Id == student.Id))
                {
                    schoolClass.Students.Add(student);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Students assigned to class successfully" });
        }

        [HttpDelete("remove-student")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> RemoveStudentFromClass([FromBody] RemoveStudentFromClassModel model)
        {
            var schoolClass = await _context.SchoolClasses
                .Include(sc => sc.Students)
                .FirstOrDefaultAsync(sc => sc.Id == model.ClassId);

            if (schoolClass == null)
            {
                return NotFound("Class not found.");
            }

            var student = await _userManager.FindByIdAsync(model.StudentId);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            if (schoolClass.Students != null && schoolClass.Students.Contains(student))
            {
                schoolClass.Students.Remove(student);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Student removed from class successfully" });
            }
            else
            {
                return BadRequest("Student not in class.");
            }
        }

        [HttpDelete("{classId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSchoolClass(int classId)
        {
            var schoolClass = await _context.SchoolClasses
                .Include(sc => sc.Students)
                .FirstOrDefaultAsync(sc => sc.Id == classId);

            if (schoolClass == null)
            {
                return NotFound("Class not found.");
            }

            _context.SchoolClasses.Remove(schoolClass);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Class deleted successfully" });
        }

        //[HttpGet("my-classes")]
        //[Authorize]
        //public async Task<IActionResult> GetMyClasses()
        //{
        //    var userId = _userManager.GetUserId(User);
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized();
        //    }

        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var myClasses = await _context.SchoolClasses
        //        .Include(sc => sc.Teacher)
        //        .Include(sc => sc.Students)
        //        .Where(sc => sc.Teacher.Id == userId || sc.Students.Any(s => s.Id == userId))
        //        .ToListAsync();

        //    return Ok(myClasses);
        //}

        //[HttpGet("{classId}/students")]
        //[Authorize]
        //public async Task<IActionResult> GetClassStudents(int classId)
        //{
        //    var userId = _userManager.GetUserId(User);
        //    var schoolClass = await _context.SchoolClasses
        //        .Include(sc => sc.Teacher)
        //        .Include(sc => sc.Students)
        //        .FirstOrDefaultAsync(sc => sc.Id == classId);

        //    if (schoolClass == null)
        //    {
        //        return NotFound("Class not found.");
        //    }

        //    if (schoolClass.Teacher.Id != userId && !schoolClass.Students.Any(s => s.Id == userId))
        //    {
        //        return Forbid();
        //    }

        //    return Ok(schoolClass.Students);
        //}

        [HttpGet("all")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> GetAllClasses()
        {
            var classes = await _context.SchoolClasses
                .Include(sc => sc.Teacher)
                .Select(sc => new
                {
                    sc.Id,
                    sc.Name,
                    sc.Description,
                    TeacherName = sc.Teacher.UserName
                })
                .ToListAsync();

            return Ok(classes);
        }

        [HttpGet("list")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListClasses()
        {
            var classes = await _context.SchoolClasses.Include(sc => sc.Teacher).Include(sc => sc.Students).ToListAsync();
            return Ok(classes);
        }
    }
}
