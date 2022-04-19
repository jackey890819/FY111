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
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
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
                entity.ToTable("group");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Icon)
                    .HasMaxLength(45)
                    .HasColumnName("icon");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.MemberHasDeviceId })
                    .HasName("PRIMARY");

                entity.ToTable("log");

                entity.HasIndex(e => e.MemberHasDeviceId, "fk_Log_Member_has_Device1_idx");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.MemberHasDeviceId).HasColumnName("Member_has_Device_id");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.HasOne(d => d.MemberHasDevice)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.MemberHasDeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Log_Member_has_Device1");
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
                entity.ToTable("member_has_device");

                entity.HasIndex(e => e.DeviceId, "fk_Member_has_Device_Device1_idx");

                entity.HasIndex(e => e.MemberId, "fk_Member_has_Device_Member1_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DeviceId).HasColumnName("Device_id");

                entity.Property(e => e.MacAddress)
                    .HasMaxLength(17)
                    .HasColumnName("mac_address");

                entity.Property(e => e.MemberId).HasColumnName("Member_id");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.MemberHasDevices)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_Device_Device1");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MemberHasDevices)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_Device_Member1");
            });

            modelBuilder.Entity<MemberHasGroup>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.GroupId })
                    .HasName("PRIMARY");

                entity.ToTable("member_has_group");

                entity.HasIndex(e => e.GroupId, "fk_Member_has_Group_Group1_idx");

                entity.HasIndex(e => e.MemberId, "fk_Member_has_Group_Member1_idx");

                entity.Property(e => e.MemberId).HasColumnName("Member_id");

                entity.Property(e => e.GroupId).HasColumnName("Group_id");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.MemberHasGroups)
                    .HasForeignKey(d => d.GroupId)
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
                entity.ToTable("metaverse");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Icon)
                    .HasMaxLength(45)
                    .HasColumnName("icon");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("ip")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
