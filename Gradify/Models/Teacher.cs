namespace Gradify.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string UserId { get; set; } // IdentityUser ID
        public List<Class> Classes { get; set; }
    }
}
