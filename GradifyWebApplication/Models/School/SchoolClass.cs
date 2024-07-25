namespace GradifyWebApplication.Models.School
{
    public class SchoolClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> Students { get; set; }
        public virtual ApplicationUser Teacher { get; set; }
    }
}
