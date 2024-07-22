namespace Gradify.Models
{
    public class ClassFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string UploadedByUserId { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}
