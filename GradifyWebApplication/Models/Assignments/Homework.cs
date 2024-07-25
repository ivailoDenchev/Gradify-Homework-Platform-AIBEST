namespace GradifyWebApplication.Models.Assignments
{
    public class Homework
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string GradingCriteria { get; set; }
        public string AttachmentUrl { get; set; }
        public string TeacherId { get; set; }
    }
}
