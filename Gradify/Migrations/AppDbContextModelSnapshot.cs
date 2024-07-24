using System;
using Gradify.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Gradify.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("Gradify.Models.Class", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("TEXT");

                b.Property<string>("Description")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<int?>("StudentId")
                    .HasColumnType("INTEGER");

                b.Property<int?>("TeacherId")
                    .HasColumnType("INTEGER");

                b.HasKey("Id");

                b.HasIndex("StudentId");

                b.HasIndex("TeacherId");

                b.ToTable("Classes");
            });

            modelBuilder.Entity("Gradify.Models.Student", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("Students");
            });

            modelBuilder.Entity("Gradify.Models.Teacher", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("Teachers");
            });

            modelBuilder.Entity("Gradify.Models.HomeworkAssignment", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("ClassId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<int>("TeacherId")
                    .HasColumnType("INTEGER");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("TEXT");

                b.Property<string>("Description")
                    .HasMaxLength(500)
                    .HasColumnType("TEXT");

                b.Property<DateTime>("DueDate")
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("ClassId");

                b.HasIndex("TeacherId");

                b.ToTable("HomeworkAssignments");
            });

            modelBuilder.Entity("Gradify.Models.SubmittedHomework", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<int>("HomeworkAssignmentId")
                    .HasColumnType("INTEGER");

                b.Property<int>("StudentId")
                    .HasColumnType("INTEGER");

                b.Property<DateTime>("SubmissionDate")
                    .HasColumnType("TEXT");

                b.Property<string>("Content")
                    .HasColumnType("TEXT");

                b.Property<int?>("Grade")
                    .HasColumnType("INTEGER");

                b.HasKey("Id");

                b.HasIndex("HomeworkAssignmentId");

                b.HasIndex("StudentId");

                b.ToTable("SubmittedHomeworks");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("TEXT");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.Property<string>("NormalizedName")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasDatabaseName("RoleNameIndex");

                b.ToTable("AspNetRoles", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("ClaimType")
                    .HasColumnType("TEXT");

                b.Property<string>("ClaimValue")
                    .HasColumnType("TEXT");

                b.Property<string>("RoleId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("TEXT");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("INTEGER");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("TEXT");

                b.Property<string>("Email")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("INTEGER");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("INTEGER");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("TEXT");

                b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.Property<string>("PasswordHash")
                    .HasColumnType("TEXT");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("TEXT");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("INTEGER");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("TEXT");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("INTEGER");

                b.Property<string>("UserName")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasDatabaseName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasDatabaseName("UserNameIndex");

                b.ToTable("AspNetUsers", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("ClaimType")
                    .HasColumnType("TEXT");

                b.Property<string>("ClaimValue")
                    .HasColumnType("TEXT");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("TEXT");

                b.Property<string>("ProviderKey")
                    .HasColumnType("TEXT");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("TEXT");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("TEXT");

                b.Property<string>("RoleId")
                    .HasColumnType("TEXT");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("TEXT");

                b.Property<string>("LoginProvider")
                    .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .HasColumnType("TEXT");

                b.Property<string>("Value")
                    .HasColumnType("TEXT");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens", (string)null);
            });

            modelBuilder.Entity("Gradify.Models.Class", b =>
            {
                b.HasOne("Gradify.Models.Student", null)
                    .WithMany("Classes")
                    .HasForeignKey("StudentId");

                b.HasOne("Gradify.Models.Teacher", null)
                    .WithMany("Classes")
                    .HasForeignKey("TeacherId");
            });

            modelBuilder.Entity("Gradify.Models.HomeworkAssignment", b =>
            {
                b.HasOne("Gradify.Models.Class", "Class")
                    .WithMany("HomeworkAssignments")
                    .HasForeignKey("ClassId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Gradify.Models.Teacher", "Teacher")
                    .WithMany("HomeworkAssignments")
                    .HasForeignKey("TeacherId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Class");

                b.Navigation("Teacher");
            });

            modelBuilder.Entity("Gradify.Models.SubmittedHomework", b =>
            {
                b.HasOne("Gradify.Models.HomeworkAssignment", "HomeworkAssignment")
                    .WithMany("SubmittedHomeworks")
                    .HasForeignKey("HomeworkAssignmentId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Gradify.Models.Student", "Student")
                    .WithMany("SubmittedHomeworks")
                    .HasForeignKey("StudentId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("HomeworkAssignment");

                b.Navigation("Student");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Gradify.Models.Class", b =>
            {
                b.Navigation("HomeworkAssignments");

                b.Navigation("ClassFiles");
            });

            modelBuilder.Entity("Gradify.Models.Student", b =>
            {
                b.Navigation("Classes");

                b.Navigation("SubmittedHomeworks");
            });

            modelBuilder.Entity("Gradify.Models.Teacher", b =>
            {
                b.Navigation("Classes");

                b.Navigation("HomeworkAssignments");
            });

            modelBuilder.Entity("Gradify.Models.HomeworkAssignment", b =>
            {
                b.Navigation("SubmittedHomeworks");
            });
#pragma warning restore 612, 618
        }
    }
}
