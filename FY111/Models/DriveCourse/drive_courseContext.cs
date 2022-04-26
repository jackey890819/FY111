using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FY111.Models.DriveCourse
{
    public partial class drive_courseContext : DbContext
    {
        public drive_courseContext()
        {
        }

        public drive_courseContext(DbContextOptions<drive_courseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseMember> CourseMembers { get; set; }
        public virtual DbSet<Examination> Examinations { get; set; }
        public virtual DbSet<DriveCourseUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Server=localhost; Port=3306;User Id=root;Password=root;Database=drive_course;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("course");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseOutline)
                    .HasMaxLength(256)
                    .HasColumnName("course_outline");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.NumberOfPeople).HasColumnName("number_of_people");

                entity.Property(e => e.Remark)
                    .HasMaxLength(20)
                    .HasColumnName("remark");
            });

            modelBuilder.Entity<CourseMember>(entity =>
            {
                entity.ToTable("course_member");

                entity.HasIndex(e => e.CourseId, "course_id_idx");

                entity.HasIndex(e => e.UserId, "user_id_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.Remark)
                    .HasMaxLength(20)
                    .HasColumnName("remark");

                entity.Property(e => e.Type)
                    .HasColumnType("tinyint")
                    .HasColumnName("type");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseMembers)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("course_member_course_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CourseMembers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("course_member_user_id");
            });

            modelBuilder.Entity<Examination>(entity =>
            {
                entity.ToTable("examination");

                entity.HasIndex(e => e.CourseId, "course_id_idx");

                entity.HasIndex(e => e.UserId, "user_id_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.Remark)
                    .HasMaxLength(20)
                    .HasColumnName("remark");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("type");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Examinations)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("examination_course_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Examinations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("examination_user_id");
            });

            modelBuilder.Entity<DriveCourseUser>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Account, "account_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Password, "password_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("account");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("password");

                entity.Property(e => e.Permission)
                    .HasColumnType("tinyint")
                    .HasColumnName("permission")
                    .HasDefaultValueSql("'2'");

                entity.Property(e => e.PersonalImg)
                    .HasMaxLength(256)
                    .HasColumnName("personal_img");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
