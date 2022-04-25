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
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Metaverse> Metaverses { get; set; }
        public virtual DbSet<MetaverseLog> MetaverseLogs { get; set; }
        public virtual DbSet<MetaverseSignIn> MetaverseSignIns { get; set; }
        public virtual DbSet<MetaverseSignUp> MetaverseSignUps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Server=localhost; Port=3306;User Id=root;Password=admin;Database=FY111;");
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
                entity.HasKey(e => new { e.MemberId, e.DeviceType })
                    .HasName("PRIMARY");

                entity.ToTable("login_log");

                entity.HasIndex(e => e.DeviceType, "fk_Log_Device1_idx");

                entity.HasIndex(e => e.MemberId, "fk_Log_Member1_idx");

                entity.Property(e => e.MemberId).HasColumnName("Member_id");

                entity.Property(e => e.DeviceType).HasColumnName("Device_type");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.HasOne(d => d.DeviceTypeNavigation)
                    .WithMany(p => p.LoginLogs)
                    .HasForeignKey(d => d.DeviceType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Log_Device1");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.LoginLogs)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Log_Member1");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("member");

                entity.HasIndex(e => e.Id, "member_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("account");

                entity.Property(e => e.Avater)
                    .HasMaxLength(45)
                    .HasColumnName("avater");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("password");

                entity.Property(e => e.Permission)
                    .HasColumnName("permission")
                    .HasDefaultValueSql("'3'");

                entity.Property(e => e.State)
                    .HasColumnType("tinyint")
                    .HasColumnName("state");
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
                entity.HasKey(e => new { e.MetaverseId, e.MemberId })
                    .HasName("PRIMARY");

                entity.ToTable("metaverse_log");

                entity.HasIndex(e => e.MemberId, "fk_Metaverse_has_Member_Member3_idx");

                entity.HasIndex(e => e.MetaverseId, "fk_Metaverse_has_Member_Metaverse3_idx");

                entity.Property(e => e.MetaverseId).HasColumnName("Metaverse_id");

                entity.Property(e => e.MemberId).HasColumnName("Member_id");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MetaverseLogs)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Metaverse_has_Member_Member3");

                entity.HasOne(d => d.Metaverse)
                    .WithMany(p => p.MetaverseLogs)
                    .HasForeignKey(d => d.MetaverseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Metaverse_has_Member_Metaverse3");
            });

            modelBuilder.Entity<MetaverseSignIn>(entity =>
            {
                entity.HasKey(e => new { e.MetaverseId, e.MemberId })
                    .HasName("PRIMARY");

                entity.ToTable("metaverse_sign_in");

                entity.HasIndex(e => e.MemberId, "fk_Metaverse_has_Member_Member2_idx");

                entity.HasIndex(e => e.MetaverseId, "fk_Metaverse_has_Member_Metaverse2_idx");

                entity.Property(e => e.MetaverseId).HasColumnName("Metaverse_id");

                entity.Property(e => e.MemberId).HasColumnName("Member_id");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MetaverseSignIns)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Metaverse_has_Member_Member2");

                entity.HasOne(d => d.Metaverse)
                    .WithMany(p => p.MetaverseSignIns)
                    .HasForeignKey(d => d.MetaverseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Metaverse_has_Member_Metaverse2");
            });

            modelBuilder.Entity<MetaverseSignUp>(entity =>
            {
                entity.HasKey(e => new { e.MetaverseId, e.MemberId })
                    .HasName("PRIMARY");

                entity.ToTable("metaverse_sign_up");

                entity.HasIndex(e => e.MemberId, "fk_Metaverse_has_Member_Member1_idx");

                entity.HasIndex(e => e.MetaverseId, "fk_Metaverse_has_Member_Metaverse1_idx");

                entity.Property(e => e.MetaverseId).HasColumnName("Metaverse_id");

                entity.Property(e => e.MemberId).HasColumnName("Member_id");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MetaverseSignUps)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Metaverse_has_Member_Member1");

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
