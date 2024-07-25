using GradifyWebApplication.Models.Assignments;
using GradifyWebApplication.Models;
using GradifyWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradifyWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Teacher")]
    public class HomeworkController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GoogleCloudStorageService _storageService;

        public HomeworkController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, GoogleCloudStorageService storageService)
        {
            _context = context;
            _userManager = userManager;
            _storageService = storageService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateHomework([FromForm] CreateHomeworkModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }

            var userName = User.Identity.Name;

            if (userName == null)
            {
                return BadRequest(new { message = "User name is null" });
            }

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            if (model.Attachment != null)
            {
                using (var stream = model.Attachment.OpenReadStream())
                {
                    var attachmentUrl = await _storageService.UploadFileAsync(stream, model.Attachment.FileName);
                    model.AttachmentUrl = attachmentUrl;
                }
            }

            var homework = new Homework
            {
                Title = model.Title,
                Description = model.Description,
                Deadline = model.Deadline,
                GradingCriteria = model.GradingCriteria,
                AttachmentUrl = model.AttachmentUrl,
                TeacherId = user.Id
            };

            _context.Homeworks.Add(homework);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Homework created successfully" });
        }

        [HttpGet("list")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> ListHomeworks()
        {
            var homeworks = await _context.Homeworks.ToListAsync();
            return Ok(homeworks);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetHomeworkDetails(int id)
        {
            var homework = await _context.Homeworks.FindAsync(id);
            if (homework == null)
            {
                return NotFound();
            }

            return Ok(homework);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UpdateHomework(int id, [FromForm] CreateHomeworkModel model)
        {
            var homework = await _context.Homeworks.FindAsync(id);
            if (homework == null)
            {
                return NotFound();
            }

            if (model.Attachment != null)
            {
                using (var stream = model.Attachment.OpenReadStream())
                {
                    var attachmentUrl = await _storageService.UploadFileAsync(stream, model.Attachment.FileName);
                    homework.AttachmentUrl = attachmentUrl;
                }
            }

            homework.Title = model.Title;
            homework.Description = model.Description;
            homework.Deadline = model.Deadline;
            homework.GradingCriteria = model.GradingCriteria;

            _context.Homeworks.Update(homework);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Homework updated successfully" });
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteHomework(int id)
        {
            var homework = await _context.Homeworks.FindAsync(id);
            if (homework == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(homework.AttachmentUrl))
            {
                var fileName = Path.GetFileName(new Uri(homework.AttachmentUrl).AbsolutePath);
                await _storageService.DeleteFileAsync(fileName);
            }

            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Homework deleted successfully" });
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AssignHomeworkToStudents([FromBody] AssignHomeworkToStudentModel model)
        {
            var homework = await _context.Homeworks.FindAsync(model.HomeworkId);
            if (homework == null)
            {
                return NotFound(new { message = "Homework not found" });
            }

            var users = await _userManager.Users
                .Where(u => model.StudentUsernames.Contains(u.UserName))
                .ToListAsync();

            if (!users.Any())
            {
                return BadRequest(new { message = "No valid students found for assignment" });
            }

            foreach (var user in users)
            {
                var assignment = new StudentHomeworkAssignment
                {
                    StudentId = user.Id,
                    HomeworkId = homework.Id,
                    AssignedDate = DateTime.UtcNow,
                    Status = "Assigned"
                };
                _context.StudentHomeworkAssignments.Add(assignment);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Homework assigned to students successfully" });
        }

        [HttpGet("submissions/{homeworkId}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetHomeworkSubmissions(int homeworkId)
        {
            var homework = await _context.Homeworks.FindAsync(homeworkId);
            if (homework == null)
            {
                return NotFound(new { message = "Homework not found" });
            }

            var submissions = await _context.StudentHomeworkAssignments
                .Where(a => a.HomeworkId == homeworkId && a.Status == "Submitted")
                .Include(a => a.Student)
                .ToListAsync();

            if (!submissions.Any())
            {
                return Ok(new { message = "No submissions found for this homework" });
            }

            var result = submissions.Select(s => new
            {
                StudentName = s.Student.UserName,
                SubmissionUrl = s.SubmissionUrl,
                SubmissionDate = s.SubmissionDate,
                Status = s.Status
            });

            return Ok(result);
        }

        [HttpGet("assigned")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetAssignedHomeworks()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return Unauthorized(new { message = "User not found or not authenticated" });
            }

            var studentAssignments = await _context.StudentHomeworkAssignments
                .Where(s => s.StudentId == user.Id)
                .Include(s => s.Homework)
                .ToListAsync();

            var result = studentAssignments.Select(s => new
            {
                s.Homework.Id,
                s.Homework.Title,
                s.Homework.Description,
                s.Homework.Deadline,
                s.Homework.GradingCriteria,
                s.Homework.AttachmentUrl,
                s.Status,
                s.AssignedDate
            });

            return Ok(result);
        }

        [HttpPost("submit")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitHomework([FromForm] SubmitHomeworkModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var assignment = await _context.StudentHomeworkAssignments
                .FirstOrDefaultAsync(a => a.HomeworkId == model.HomeworkId && a.StudentId == user.Id);

            if (assignment == null)
            {
                return BadRequest(new { message = "Assignment not found for this user" });
            }

            if (model.SubmissionFile != null)
            {
                using (var stream = model.SubmissionFile.OpenReadStream())
                {
                    var submissionUrl = await _storageService.UploadFileAsync(stream, model.SubmissionFile.FileName);
                    assignment.Status = "Submitted";
                    assignment.SubmissionUrl = submissionUrl;
                }

                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Homework submitted successfully" });
        }

    }
}
