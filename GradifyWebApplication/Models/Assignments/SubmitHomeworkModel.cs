namespace GradifyWebApplication.Models.Assignments
{
    public class SubmitHomeworkModel
    {
        public int HomeworkId { get; set; }
        public IFormFile SubmissionFile { get; set; }
    }
}
