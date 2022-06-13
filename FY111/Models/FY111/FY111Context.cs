using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FY111.Models.FY111
{
    public partial class FY111Context : DbContext
    {
        public FY111Context()
        {
        }

        public FY111Context(DbContextOptions<FY111Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<ClassCheckin> ClassCheckins { get; set; }
        public virtual DbSet<ClassLittleunit> ClassLittleunits { get; set; }
        public virtual DbSet<ClassLog> ClassLogs { get; set; }
        public virtual DbSet<ClassQuestion> ClassQuestions { get; set; }
        public virtual DbSet<ClassSignup> ClassSignups { get; set; }
        public virtual DbSet<ClassUnit> ClassUnits { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }
        public virtual DbSet<LoginLog> LoginLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("Server=localhost; Port=3306;User Id=root;Password=root;Database=FY111;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("class");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CheckinEnabled)
                    .HasColumnType("tinyint")
                    .HasColumnName("checkin_enabled");

                entity.Property(e => e.Code)
                    .HasMaxLength(45)
                    .HasColumnName("code");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.Image)
                    .HasMaxLength(45)
                    .HasColumnName("image");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("ip")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.SignupEnabled)
                    .HasColumnType("tinyint")
                    .HasColumnName("signup_enabled");
            });

            modelBuilder.Entity<ClassCheckin>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.ClassId })
                    .HasName("PRIMARY");

                entity.ToTable("class_checkin");

                entity.HasIndex(e => e.ClassId, "fk_Metaverse_has_Member_Metaverse1_idx");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassCheckins)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Metaverse_has_Member_Metaverse1");
            });

            modelBuilder.Entity<ClassLittleunit>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ClassUnitId })
                    .HasName("PRIMARY");

                entity.ToTable("class_littleunit");

                entity.HasIndex(e => e.ClassUnitId, "fk_class_littleunit_class_unit1");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.ClassUnitId).HasColumnName("Class_unit_id");

                entity.Property(e => e.Code)
                    .HasMaxLength(45)
                    .HasColumnName("code");

                entity.Property(e => e.Image)
                    .HasMaxLength(45)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<ClassLog>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.StartTime })
                    .HasName("PRIMARY");

                entity.ToTable("class_log");

                entity.HasIndex(e => e.ClassId, "fk_Metaverse_Log_Metaverse1_idx");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassLogs)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Metaverse_Log_Metaverse1");
            });

            modelBuilder.Entity<ClassQuestion>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ClassId })
                    .HasName("PRIMARY");

                entity.ToTable("class_question");

                entity.HasIndex(e => e.ClassId, "fk_class_question_class1");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.Property(e => e.Discription)
                    .HasMaxLength(100)
                    .HasColumnName("discription");

                entity.Property(e => e.Option).HasColumnName("option");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassQuestions)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_class_question_class1");
            });

            modelBuilder.Entity<ClassSignup>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.ClassId })
                    .HasName("PRIMARY");

                entity.ToTable("class_signup");

                entity.HasIndex(e => e.ClassId, "fk_Metaverse_has_Member_Metaverse2_idx");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassSignups)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Metaverse_has_Member_Metaverse2");
            });

            modelBuilder.Entity<ClassUnit>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ClassId })
                    .HasName("PRIMARY");

                entity.ToTable("class_unit");

                entity.HasIndex(e => e.ClassId, "fk_class_unit_class1");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.Property(e => e.Code)
                    .HasMaxLength(45)
                    .HasColumnName("code");

                entity.Property(e => e.Image)
                    .HasMaxLength(45)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassUnits)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_class_unit_class1");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("device");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Icon)
                    .HasMaxLength(45)
                    .HasColumnName("icon");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Efmigrationshistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId)
                    .HasName("PRIMARY");

                entity.ToTable("__efmigrationshistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<LoginLog>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.StartTime })
                    .HasName("PRIMARY");

                entity.ToTable("login_log");

                entity.HasIndex(e => e.DeviceType, "fk_Login_Log_Device1_idx");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DeviceType).HasColumnName("Device_type");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.HasOne(d => d.DeviceTypeNavigation)
                    .WithMany(p => p.LoginLogs)
                    .HasForeignKey(d => d.DeviceType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Login_Log_Device1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
