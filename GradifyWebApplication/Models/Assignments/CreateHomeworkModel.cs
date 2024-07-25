namespace GradifyWebApplication.Models.Assignments
{
    public class CreateHomeworkModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string GradingCriteria { get; set; }
        public IFormFile Attachment { get; set; }
        public string AttachmentUrl { get; set; }
    }
}
