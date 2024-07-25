using GradifyWebApplication.Models.Assignments;
using GradifyWebApplication.Models.School;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GradifyWebApplication.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}

        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<StudentHomeworkAssignment> StudentHomeworkAssignments { get; set; }
        public DbSet<StudentGrade> StudentGrades { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StudentHomeworkAssignment>()
                .HasOne(sha => sha.Homework)
                .WithMany()
                .HasForeignKey(sha => sha.HomeworkId);

            builder.Entity<SchoolClass>()
                .HasMany(sc => sc.Students)
                .WithMany("SchoolClasses");
        }
    }
}
