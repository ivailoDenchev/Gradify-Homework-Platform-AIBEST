namespace GradifyWebApplication.Models.Assignments
{
    public class StudentHomeworkAssignment
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public int HomeworkId { get; set; }
        public DateTime AssignedDate { get; set; }
        public string Status { get; set; }
        public string? SubmissionUrl { get; set; }
        public DateTime? SubmissionDate { get; set; }

        public virtual ApplicationUser Student { get; set; }
        public virtual Homework Homework { get; set; }
    }
}
