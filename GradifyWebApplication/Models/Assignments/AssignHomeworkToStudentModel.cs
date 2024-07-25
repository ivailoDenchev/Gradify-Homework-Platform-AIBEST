namespace GradifyWebApplication.Models.Assignments
{
    public class AssignHomeworkToStudentModel
    {
        public int HomeworkId { get; set; }
        public List<string> StudentUsernames { get; set; }
    }
}
