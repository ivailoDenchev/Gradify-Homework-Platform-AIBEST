namespace GradifyWebApplication.Models.School
{
    public class AssignStudentsToClassModel
    {
        public int ClassId { get; set; }
        public List<string> StudentIds { get; set; }
    }
}
