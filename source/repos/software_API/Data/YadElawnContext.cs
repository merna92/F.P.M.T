using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace software_API.Data;

public partial class YadElawnContext : DbContext
{
    public YadElawnContext()
    {
    }

    public YadElawnContext(DbContextOptions<YadElawnContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Beneficiary> Beneficiaries { get; set; }

    public virtual DbSet<Charity> Charities { get; set; }

    public virtual DbSet<Clothe> Clothes { get; set; }

    public virtual DbSet<Donation> Donations { get; set; }

    public virtual DbSet<Donor> Donors { get; set; }

    public virtual DbSet<Evaluate> Evaluates { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MedicalSupply> MedicalSupplies { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<StatusHistory> StatusHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwAvailableFoodDonation> VwAvailableFoodDonations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-I3GABO5\\SQLEXPRESS;Database=yad_elawn;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE4E8A25B80AD");

            entity.ToTable("Admin");

            entity.Property(e => e.AdminId)
                .ValueGeneratedNever()
                .HasColumnName("AdminID");

            entity.HasOne(d => d.AdminNavigation).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.AdminId)
                .HasConstraintName("FK_Admin_User");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Audit_Lo__5E5499A8BFB110E1");

            entity.ToTable("Audit_Log");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Action_Date");
            entity.Property(e => e.ActionTaken).HasColumnName("Action_Taken");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");

            entity.HasOne(d => d.Admin).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK_AuditLog_Admin");
        });

        modelBuilder.Entity<Beneficiary>(entity =>
        {
            entity.HasKey(e => e.BeneficiaryId).HasName("PK__Benefici__3FBA95D5EB8886E6");

            entity.ToTable("Beneficiary");

            entity.Property(e => e.BeneficiaryId)
                .ValueGeneratedNever()
                .HasColumnName("BeneficiaryID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");

            entity.HasOne(d => d.BeneficiaryNavigation).WithOne(p => p.Beneficiary)
                .HasForeignKey<Beneficiary>(d => d.BeneficiaryId)
                .HasConstraintName("FK_Beneficiary_User");

            entity.HasOne(d => d.Location).WithMany(p => p.Beneficiaries)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_Beneficiary_Location");
        });

        modelBuilder.Entity<Charity>(entity =>
        {
            entity.HasKey(e => e.CharityId).HasName("PK__Charity__8189E5B412617A70");

            entity.ToTable("Charity");

            entity.HasIndex(e => e.LicenseNumber, "UQ__Charity__9ED5EDA2AFEEC5DE").IsUnique();

            entity.Property(e => e.CharityId)
                .ValueGeneratedNever()
                .HasColumnName("CharityID");
            entity.Property(e => e.CoverageArea)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LicenseNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("License_Number");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");

            entity.HasOne(d => d.CharityNavigation).WithOne(p => p.Charity)
                .HasForeignKey<Charity>(d => d.CharityId)
                .HasConstraintName("FK_Charity_User");

            entity.HasOne(d => d.Location).WithMany(p => p.Charities)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_Charity_Location");
        });

        modelBuilder.Entity<Clothe>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__Clothes__C5082EDBC4260758");

            entity.Property(e => e.DonationId)
                .ValueGeneratedNever()
                .HasColumnName("DonationID");
            entity.Property(e => e.CleaningStatus)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Cleaning_Status");
            entity.Property(e => e.Condition)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Season)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Size)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Donation).WithOne(p => p.Clothe)
                .HasForeignKey<Clothe>(d => d.DonationId)
                .HasConstraintName("FK_Clothes_Donation");
        });

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__Donation__C5082EDB669CDD92");

            entity.ToTable("Donation", tb => tb.HasTrigger("trg_DonationStatusHistory"));

            entity.Property(e => e.DonationId).HasColumnName("DonationID");
            entity.Property(e => e.DonorId).HasColumnName("DonorID");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Donor).WithMany(p => p.Donations)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donation_Donor");

            entity.HasOne(d => d.Location).WithMany(p => p.Donations)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_Donation_Location");
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.DonorId).HasName("PK__Donor__052E3F98D6FD6F36");

            entity.ToTable("Donor");

            entity.Property(e => e.DonorId)
                .ValueGeneratedNever()
                .HasColumnName("DonorID");
            entity.Property(e => e.DonationCount)
                .HasDefaultValue(0)
                .HasColumnName("Donation_Count");

            entity.HasOne(d => d.DonorNavigation).WithOne(p => p.Donor)
                .HasForeignKey<Donor>(d => d.DonorId)
                .HasConstraintName("FK_Donor_User");
        });

        modelBuilder.Entity<Evaluate>(entity =>
        {
            entity.HasKey(e => new { e.CharityId, e.DonorId }).HasName("PK__Evaluate__11DB064DF559C94A");

            entity.Property(e => e.CharityId).HasColumnName("CharityID");
            entity.Property(e => e.DonorId).HasColumnName("DonorID");

            entity.HasOne(d => d.Charity).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.CharityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evaluates_Charity");

            entity.HasOne(d => d.Donor).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evaluates_Donor");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__Food__C5082EDB8210D6F5");

            entity.ToTable("Food");

            entity.Property(e => e.DonationId)
                .ValueGeneratedNever()
                .HasColumnName("DonationID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryDate).HasColumnName("Expiry_Date");
            entity.Property(e => e.FoodType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Food_Type");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Product_Name");
            entity.Property(e => e.Quantity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShelfLife)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Shelf_Life");
            entity.Property(e => e.StorageCondition)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Storage_Condition");

            entity.HasOne(d => d.Donation).WithOne(p => p.Food)
                .HasForeignKey<Food>(d => d.DonationId)
                .HasConstraintName("FK_Food_Donation");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA477CA9E9C85");

            entity.ToTable("Location");

            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.CityArea)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("City_Area");
            entity.Property(e => e.GpsCoordinates)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GPS_Coordinates");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.MatchId).HasName("PK__Matches__4218C8373E5E2807");

            entity.Property(e => e.MatchId).HasColumnName("MatchID");
            entity.Property(e => e.BeneficiaryId).HasColumnName("BeneficiaryID");
            entity.Property(e => e.CharityId).HasColumnName("CharityID");
            entity.Property(e => e.Distance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DonationId).HasColumnName("DonationID");
            entity.Property(e => e.MatchDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Match_Date");
            entity.Property(e => e.UrgencyLevel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Urgency_Level");

            entity.HasOne(d => d.Beneficiary).WithMany(p => p.Matches)
                .HasForeignKey(d => d.BeneficiaryId)
                .HasConstraintName("FK_Matches_Beneficiary");

            entity.HasOne(d => d.Charity).WithMany(p => p.Matches)
                .HasForeignKey(d => d.CharityId)
                .HasConstraintName("FK_Matches_Charity");

            entity.HasOne(d => d.Donation).WithMany(p => p.Matches)
                .HasForeignKey(d => d.DonationId)
                .HasConstraintName("FK_Matches_Donation");
        });

        modelBuilder.Entity<MedicalSupply>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__Medical___C5082EDB316C6D4A");

            entity.ToTable("Medical_Supplies");

            entity.Property(e => e.DonationId)
                .ValueGeneratedNever()
                .HasColumnName("DonationID");
            entity.Property(e => e.Condition)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Quantity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SupplyName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Supply_Name");

            entity.HasOne(d => d.Donation).WithOne(p => p.MedicalSupply)
                .HasForeignKey<MedicalSupply>(d => d.DonationId)
                .HasConstraintName("FK_MedicalSupplies_Donation");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK__Medicine__C5082EDB7CD61DE6");

            entity.ToTable("Medicine");

            entity.Property(e => e.DonationId)
                .ValueGeneratedNever()
                .HasColumnName("DonationID");
            entity.Property(e => e.ExpiryDate).HasColumnName("Expiry_Date");
            entity.Property(e => e.MedicineName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Medicine_Name");
            entity.Property(e => e.MedicineType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Medicine_Type");
            entity.Property(e => e.Quantity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SafetyConditions).HasColumnName("Safety_Conditions");

            entity.HasOne(d => d.Donation).WithOne(p => p.Medicine)
                .HasForeignKey<Medicine>(d => d.DonationId)
                .HasConstraintName("FK_Medicine_Donation");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Message__C87C037CF447A4DF");

            entity.ToTable("Message");

            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("Is_Read");
            entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.SentDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Sent_Date_Time");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .HasConstraintName("FK_Message_Receiver");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK_Message_Sender");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotifId).HasName("PK__Notifica__DDBFF31376979DCE");

            entity.ToTable("Notification");

            entity.Property(e => e.NotifId).HasColumnName("NotifID");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("Is_Read");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Notification_User");
        });

        modelBuilder.Entity<StatusHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__Status_H__4D7B4ADDE88A99A3");

            entity.ToTable("Status_History");

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.ChangeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Change_Date");
            entity.Property(e => e.DonationId).HasColumnName("DonationID");
            entity.Property(e => e.NewStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("New_Status");
            entity.Property(e => e.OldStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Old_Status");

            entity.HasOne(d => d.Donation).WithMany(p => p.StatusHistories)
                .HasForeignKey(d => d.DonationId)
                .HasConstraintName("FK_StatusHistory_Donation");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC45C7CA16");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D105347C5A97F5").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FName");
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LName");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserType).HasMaxLength(50);
        });

        modelBuilder.Entity<VwAvailableFoodDonation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_AvailableFoodDonations");

            entity.Property(e => e.CityArea)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("City_Area");
            entity.Property(e => e.DonationId).HasColumnName("DonationID");
            entity.Property(e => e.DonorName)
                .HasMaxLength(101)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryDate).HasColumnName("Expiry_Date");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Product_Name");
            entity.Property(e => e.Quantity)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
