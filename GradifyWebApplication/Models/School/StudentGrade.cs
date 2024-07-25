namespace GradifyWebApplication.Models.School
{
    public class StudentGrade
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string Grade { get; set; }
        public string Description { get; set; }
        public string TeacherId { get; set; }
        public DateTime DateAssigned { get; set; }

        public virtual ApplicationUser Student { get; set; }
        public virtual ApplicationUser Teacher { get; set; }
    }
}
