﻿using System;
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
        public virtual DbSet<ClassLittleunit> ClassLittleunits { get; set; }
        public virtual DbSet<ClassLog> ClassLogs { get; set; }
        public virtual DbSet<ClassSignup> ClassSignups { get; set; }
        public virtual DbSet<ClassUnit> ClassUnits { get; set; }
        public virtual DbSet<ClassUnitApp> ClassUnitApps { get; set; }
        public virtual DbSet<ClassUnitCkpt> ClassUnitCkpts { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<LoginApp> LoginApps { get; set; }
        public virtual DbSet<LoginLog> LoginLogs { get; set; }
        public virtual DbSet<Occdisaster> Occdisasters { get; set; }
        public virtual DbSet<OperationCheckpoint> OperationCheckpoints { get; set; }
        public virtual DbSet<OperationLittleunitLog> OperationLittleunitLogs { get; set; }
        public virtual DbSet<OperationOccdisaster> OperationOccdisasters { get; set; }
        public virtual DbSet<OperationUnitLog> OperationUnitLogs { get; set; }
        public virtual DbSet<Timer> Timers { get; set; }
        public virtual DbSet<TrainingCheckin> TrainingCheckins { get; set; }
        public virtual DbSet<TrainingSignup> TrainingSignups { get; set; }
        public virtual DbSet<training> training { get; set; }

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
                    .HasColumnName("checkin_enabled")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Code)
                    .HasMaxLength(45)
                    .HasColumnName("code");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.Image)
                    .HasMaxLength(45)
                    .HasColumnName("image");

                entity.Property(e => e.Ip)
                    .HasMaxLength(15)
                    .HasColumnName("ip")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.SignupEnabled)
                    .HasColumnType("tinyint")
                    .HasColumnName("signup_enabled")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<ClassLittleunit>(entity =>
            {
                entity.ToTable("class_littleunit");

                entity.HasIndex(e => e.ClassUnitId, "fk_class_littleunit_class_unit1");

                entity.Property(e => e.Id).HasColumnName("id");

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

                entity.HasOne(d => d.ClassUnit)
                    .WithMany(p => p.ClassLittleunits)
                    .HasForeignKey(d => d.ClassUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_class_littleunit_class_unit1");
            });

            modelBuilder.Entity<ClassLog>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.StartTime })
                    .HasName("PRIMARY");

                entity.ToTable("class_log");

                entity.HasIndex(e => e.ClassId, "fk_class_log_Class1_idx");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassLogs)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_class_log_Class1");
            });

            modelBuilder.Entity<ClassSignup>(entity =>
            {
                entity.HasKey(e => new { e.TrainingSignupId, e.ClassId })
                    .HasName("PRIMARY");

                entity.ToTable("class_signup");

                entity.HasIndex(e => e.ClassId, "fk_class_signup_Class1_idx");

                entity.Property(e => e.TrainingSignupId).HasColumnName("training_signup_id");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassSignups)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_class_signup_Class1");

                entity.HasOne(d => d.TrainingSignup)
                    .WithMany(p => p.ClassSignups)
                    .HasForeignKey(d => d.TrainingSignupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_class_signup_training_signup1");
            });

            modelBuilder.Entity<ClassUnit>(entity =>
            {
                entity.ToTable("class_unit");

                entity.HasIndex(e => e.ClassId, "fk_class_unit_class1");

                entity.Property(e => e.Id).HasColumnName("id");

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

            modelBuilder.Entity<ClassUnitApp>(entity =>
            {
                entity.ToTable("class_unit_app");

                entity.HasIndex(e => e.ClassUnitId, "fk_table1_Class_unit1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Application)
                    .HasMaxLength(45)
                    .HasColumnName("application");

                entity.Property(e => e.ClassUnitId).HasColumnName("Class_unit_id");

                entity.Property(e => e.Content)
                    .HasMaxLength(45)
                    .HasColumnName("content");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.HasOne(d => d.ClassUnit)
                    .WithMany(p => p.ClassUnitApps)
                    .HasForeignKey(d => d.ClassUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_table1_Class_unit1");
            });

            modelBuilder.Entity<ClassUnitCkpt>(entity =>
            {
                entity.HasKey(e => new { e.ClassLittleunitId, e.CkptId })
                    .HasName("PRIMARY");

                entity.ToTable("class_unit_ckpt");

                entity.HasIndex(e => e.ClassLittleunitId, "fk_Class_unit_ckpt_class_littleunit1_idx");

                entity.Property(e => e.ClassLittleunitId).HasColumnName("class_littleunit_id");

                entity.Property(e => e.CkptId)
                    .HasMaxLength(45)
                    .HasColumnName("CKPT_id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.HasOne(d => d.ClassLittleunit)
                    .WithMany(p => p.ClassUnitCkpts)
                    .HasForeignKey(d => d.ClassLittleunitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Class_unit_ckpt_class_littleunit1");
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

            modelBuilder.Entity<LoginApp>(entity =>
            {
                entity.ToTable("login_app");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Application)
                    .HasMaxLength(45)
                    .HasColumnName("application");

                entity.Property(e => e.Content)
                    .HasMaxLength(45)
                    .HasColumnName("content");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");
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

            modelBuilder.Entity<Occdisaster>(entity =>
            {
                entity.HasKey(e => new { e.Code, e.ClassLittleunitId })
                    .HasName("PRIMARY");

                entity.ToTable("occdisaster");

                entity.HasIndex(e => e.ClassLittleunitId, "fk_Occdisaster_class_littleunit1_idx");

                entity.Property(e => e.Code).HasMaxLength(45);

                entity.Property(e => e.ClassLittleunitId).HasColumnName("class_littleunit_id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.HasOne(d => d.ClassLittleunit)
                    .WithMany(p => p.Occdisasters)
                    .HasForeignKey(d => d.ClassLittleunitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Occdisaster_class_littleunit1");
            });

            modelBuilder.Entity<OperationCheckpoint>(entity =>
            {
                entity.HasKey(e => new { e.OperationLittleunitLogId, e.CkptId })
                    .HasName("PRIMARY");

                entity.ToTable("operation_checkpoint");

                entity.HasIndex(e => e.OperationLittleunitLogId, "fk_operation_checkpoint_operation_littleunit_log1_idx");

                entity.Property(e => e.OperationLittleunitLogId).HasColumnName("operation_littleunit_log_id");

                entity.Property(e => e.CkptId)
                    .HasMaxLength(45)
                    .HasColumnName("CKPT_id");

                entity.HasOne(d => d.OperationLittleunitLog)
                    .WithMany(p => p.OperationCheckpoints)
                    .HasForeignKey(d => d.OperationLittleunitLogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_operation_checkpoint_operation_littleunit_log1");
            });

            modelBuilder.Entity<OperationLittleunitLog>(entity =>
            {
                entity.ToTable("operation_littleunit_log");

                entity.HasIndex(e => e.OperationLogId, "fk_operation_checkpoint_operation_log1");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.LittleunitCode)
                    .HasMaxLength(45)
                    .HasColumnName("littleunit_code");

                entity.Property(e => e.OperationLogId).HasColumnName("operation_log_id");

                entity.Property(e => e.Pass)
                    .HasColumnType("tinyint")
                    .HasColumnName("pass");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.HasOne(d => d.OperationLog)
                    .WithMany(p => p.OperationLittleunitLogs)
                    .HasForeignKey(d => d.OperationLogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_operation_checkpoint_operation_log1");
            });

            modelBuilder.Entity<OperationOccdisaster>(entity =>
            {
                entity.HasKey(e => new { e.OperationLittleunitLogId, e.OccDisasterCode })
                    .HasName("PRIMARY");

                entity.ToTable("operation_occdisaster");

                entity.Property(e => e.OperationLittleunitLogId).HasColumnName("operation_littleunit_log_id");

                entity.Property(e => e.OccDisasterCode)
                    .HasMaxLength(45)
                    .HasColumnName("OccDisaster_code");

                entity.HasOne(d => d.OperationLittleunitLog)
                    .WithMany(p => p.OperationOccdisasters)
                    .HasForeignKey(d => d.OperationLittleunitLogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_table1_operation_littleunit_log1");
            });

            modelBuilder.Entity<OperationUnitLog>(entity =>
            {
                entity.ToTable("operation_unit_log");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.Pass)
                    .HasColumnType("tinyint")
                    .HasColumnName("pass");

                entity.Property(e => e.UnitCode)
                    .HasMaxLength(45)
                    .HasColumnName("unit_code");
            });

            modelBuilder.Entity<Timer>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.StartTime })
                    .HasName("PRIMARY");

                entity.ToTable("timer");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.Property(e => e.EndTime).HasColumnName("end_time");
            });

            modelBuilder.Entity<TrainingCheckin>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.TrainingId })
                    .HasName("PRIMARY");

                entity.ToTable("training_checkin");

                entity.HasIndex(e => e.TrainingId, "fk_class_checkin_training1_idx");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.TrainingId).HasColumnName("training_id");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.TrainingCheckins)
                    .HasForeignKey(d => d.TrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_class_checkin_training1");
            });

            modelBuilder.Entity<TrainingSignup>(entity =>
            {
                entity.ToTable("training_signup");

                entity.HasIndex(e => e.TrainingId, "fk_class_signup_training1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.MemberId)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.TrainingId).HasColumnName("training_id");

                entity.HasOne(d => d.Training)
                    .WithMany(p => p.TrainingSignups)
                    .HasForeignKey(d => d.TrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_class_signup_training1");
            });

            modelBuilder.Entity<training>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.StartTime).HasColumnName("start_time");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
