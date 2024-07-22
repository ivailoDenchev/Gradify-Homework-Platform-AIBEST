namespace Gradify.Models
{
    public class Class
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ClassFile> ClassFiles { get; set; }
    }
}
