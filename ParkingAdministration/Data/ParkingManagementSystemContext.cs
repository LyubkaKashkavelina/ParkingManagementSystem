using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ParkingAdministration.Data
{
    public partial class ParkingManagementSystemContext : DbContext
    {
        public ParkingManagementSystemContext()
        {
        }

        public ParkingManagementSystemContext(DbContextOptions<ParkingManagementSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<FreeParkingSpace> FreeParkingSpaces { get; set; }
        public virtual DbSet<ParkingSpace> ParkingSpaces { get; set; }
        public virtual DbSet<ParkingSpaceFee> ParkingSpaceFees { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserToParkingSpace> UserToParkingSpaces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-LGMEH2L\\SQLSERVER2017;Database=ParkingManagementSystem;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.BookingId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.MonthlyFee).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ParkingSpaceId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.ParkingSpace)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.ParkingSpaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car");

                entity.Property(e => e.CarId).ValueGeneratedNever();

                entity.Property(e => e.CarNumber)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.Color)
                    .HasMaxLength(64);

                entity.Property(e => e.Make)
                    .HasMaxLength(64);

                entity.Property(e => e.Model)
                    .HasMaxLength(64);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FreeParkingSpace>(entity =>
            {
                entity.ToTable("FreeParkingSpace");

                entity.Property(e => e.FreeParkingSpaceId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UserSpaceId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.UserSpace)
                    .WithMany(p => p.FreeParkingSpaces)
                    .HasForeignKey(d => d.UserSpaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ParkingSpace>(entity =>
            {
                entity.ToTable("ParkingSpace");

                entity.Property(e => e.ParkingSpaceId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ParkingSpaceFeeId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ParkingSpaceNumber)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.HasOne(d => d.ParkingSpaceFee)
                    .WithMany(p => p.ParkingSpaces)
                    .HasForeignKey(d => d.ParkingSpaceFeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ParkingSpaceFee>(entity =>
            {
                entity.ToTable("ParkingSpaceFee");

                entity.Property(e => e.ParkingSpaceFeeId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.MonthlyFee).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.PaidAmount).HasColumnType("decimal(7, 2)");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("Token");

                entity.Property(e => e.TokenId).ValueGeneratedNever();

                entity.Property(e => e.UserToken).HasMaxLength(512);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.PhoneNumber).HasMaxLength(32);
            });

            modelBuilder.Entity<UserToParkingSpace>(entity =>
            {
                entity.ToTable("UserToParkingSpace");

                entity.Property(e => e.UserToParkingSpaceId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.ParkingSpaceId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.ParkingSpace)
                    .WithMany(p => p.UserToParkingSpaces)
                    .HasForeignKey(d => d.ParkingSpaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserToParkingSpaces)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSpace_User_UserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
