namespace Gradify.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string UserId { get; set; } // IdentityUser ID
        public List<Class> Classes { get; set; }
    }
}
