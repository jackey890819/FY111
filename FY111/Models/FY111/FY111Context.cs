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

        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<LoginLog> LoginLogs { get; set; }
        public virtual DbSet<Metaverse> Metaverses { get; set; }
        public virtual DbSet<MetaverseLog> MetaverseLogs { get; set; }
        public virtual DbSet<MetaverseSignIn> MetaverseSignIns { get; set; }
        public virtual DbSet<MetaverseSignUp> MetaverseSignUps { get; set; }

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

            modelBuilder.Entity<Metaverse>(entity =>
            {
                entity.ToTable("metaverse");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.Icon)
                    .HasMaxLength(45)
                    .HasColumnName("icon");

                entity.Property(e => e.Introduction).HasColumnName("introduction");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("ip")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.SigninEnabled)
                    .HasColumnType("tinyint")
                    .HasColumnName("signin_enabled");

                entity.Property(e => e.SignupEnabled)
                    .HasColumnType("tinyint")
                    .HasColumnName("signup_enabled");
            });

            modelBuilder.Entity<MetaverseLog>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.StartTime })
                    .HasName("PRIMARY");

                entity.ToTable("metaverse_log");

                entity.HasIndex(e => e.MetaverseId, "fk_Metaverse_Log_Metaverse1_idx");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.MetaverseId).HasColumnName("Metaverse_id");

                entity.HasOne(d => d.Metaverse)
                    .WithMany(p => p.MetaverseLogs)
                    .HasForeignKey(d => d.MetaverseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Metaverse_Log_Metaverse1");
            });

            modelBuilder.Entity<MetaverseSignIn>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.MetaverseId })
                    .HasName("PRIMARY");

                entity.ToTable("metaverse_sign_in");

                entity.HasIndex(e => e.MetaverseId, "fk_Metaverse_has_Member_Metaverse2_idx");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.MetaverseId).HasColumnName("Metaverse_id");

                entity.HasOne(d => d.Metaverse)
                    .WithMany(p => p.MetaverseSignIns)
                    .HasForeignKey(d => d.MetaverseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Metaverse_has_Member_Metaverse2");
            });

            modelBuilder.Entity<MetaverseSignUp>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.MetaverseId })
                    .HasName("PRIMARY");

                entity.ToTable("metaverse_sign_up");

                entity.HasIndex(e => e.MetaverseId, "fk_Metaverse_has_Member_Metaverse1_idx");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(256)
                    .HasColumnName("Member_id");

                entity.Property(e => e.MetaverseId).HasColumnName("Metaverse_id");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Metaverse)
                    .WithMany(p => p.MetaverseSignUps)
                    .HasForeignKey(d => d.MetaverseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Metaverse_has_Member_Metaverse1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
