using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FY111.Models
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
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberHasDevice> MemberHasDevices { get; set; }
        public virtual DbSet<MemberHasGroup> MemberHasGroups { get; set; }
        public virtual DbSet<Metaverse> Metaverses { get; set; }

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
                entity.HasKey(e => e.Type)
                    .HasName("PRIMARY");

                entity.ToTable("device");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Icon)
                    .HasMaxLength(45)
                    .HasColumnName("icon");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.MemberId1 })
                    .HasName("PRIMARY");

                entity.ToTable("friend");

                entity.HasIndex(e => e.MemberId, "fk_Member_has_Member_Member1_idx");

                entity.HasIndex(e => e.MemberId1, "fk_Member_has_Member_Member2_idx");

                entity.Property(e => e.MemberId).HasColumnName("Member_id");

                entity.Property(e => e.MemberId1).HasColumnName("Member_id1");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.FriendMembers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_Member_Member1");

                entity.HasOne(d => d.MemberId1Navigation)
                    .WithMany(p => p.FriendMemberId1Navigations)
                    .HasForeignKey(d => d.MemberId1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_Member_Member2");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PRIMARY");

                entity.ToTable("group");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Icon)
                    .HasMaxLength(45)
                    .HasColumnName("icon");
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

                entity.Property(e => e.Permission).HasColumnName("permission");
            });

            modelBuilder.Entity<MemberHasDevice>(entity =>
            {
                entity.HasKey(e => new { e.DeviceType, e.MemberId })
                    .HasName("PRIMARY");

                entity.ToTable("member_has_device");

                entity.HasIndex(e => e.DeviceType, "fk_Device_has_Member_Device1_idx");

                entity.HasIndex(e => e.MemberId, "fk_Device_has_Member_Member1_idx");

                entity.Property(e => e.DeviceType).HasColumnName("Device_type");

                entity.Property(e => e.MemberId).HasColumnName("Member_id");

                entity.HasOne(d => d.DeviceTypeNavigation)
                    .WithMany(p => p.MemberHasDevices)
                    .HasForeignKey(d => d.DeviceType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Device_has_Member_Device1");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MemberHasDevices)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Device_has_Member_Member1");
            });

            modelBuilder.Entity<MemberHasGroup>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.GroupName })
                    .HasName("PRIMARY");

                entity.ToTable("member_has_group");

                entity.HasIndex(e => e.GroupName, "fk_Member_has_Group_Group1_idx");

                entity.HasIndex(e => e.MemberId, "fk_Member_has_Group_Member1_idx");

                entity.Property(e => e.MemberId).HasColumnName("Member_id");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(45)
                    .HasColumnName("Group_name");

                entity.HasOne(d => d.GroupNameNavigation)
                    .WithMany(p => p.MemberHasGroups)
                    .HasForeignKey(d => d.GroupName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_Group_Group1");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MemberHasGroups)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_Group_Member1");
            });

            modelBuilder.Entity<Metaverse>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PRIMARY");

                entity.ToTable("metaverse");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Icon)
                    .HasMaxLength(45)
                    .HasColumnName("icon");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("ip")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
