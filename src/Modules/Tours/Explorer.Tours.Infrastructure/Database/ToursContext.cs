using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.TourExecutions;
using Explorer.Tours.Core.Domain.Tours;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Explorer.Tours.Infrastructure.Database;

public class ToursContext : DbContext
{
    
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<MapObject> MapObjects { get; set; }
    public DbSet<Checkpoint> Checkpoints { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourEquipment> TourEquipment { get; set; }
    public DbSet<TourPreference> TourPreference { get; set; }
    public DbSet<ReportedIssue> ReportedIssues { get; set; }
    public DbSet<TourRating> TourRating { get; set; }
    public DbSet<PublicCheckpoint> PublicCheckpoint { get; set; }
    public DbSet<PublicMapObject> PublicMapObjects { get; set; }
    public DbSet<TouristPosition> TouristPosition { get; set; }
    public DbSet<TourExecution> TourExecution { get; set; }
    public DbSet<TourBundle> TourBundles { get; set; }
    public DbSet<TourTourBundle> TourTourBundles { get; set; }

    public ToursContext(DbContextOptions<ToursContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.HasDefaultSchema("tours");
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.Entity<Tour>()
           .HasMany(t => t.Checkpoints)
           .WithOne(t => t.Tour)
           .HasForeignKey(t => t.TourId)
           .IsRequired();


        modelBuilder.Entity<TourExecution>()
           .HasMany(t => t.CompletedCheckpoints)
           .WithOne()
           .HasForeignKey(t => t.TourExecutionId)
           .IsRequired();

        modelBuilder.Entity<TourExecution>()
            .HasOne(t => t.Tour)
            .WithMany()
            .HasForeignKey(t => t.TourId)
            .IsRequired();

        modelBuilder.Entity<Tour>()
            .HasMany(t => t.Equipment)
            .WithMany()
            .UsingEntity<TourEquipment>();
        ConfigureReportedIssues(modelBuilder);
        ConfigureTourRatings(modelBuilder);

        modelBuilder.Entity<ReportedIssue>().Property(item => item.Comments).HasColumnType("jsonb");
        modelBuilder.Entity<TourExecution>()
        .Property(t => t.Changes)
        .HasConversion(
            v => TourExecutionEventsConverter.Write(v),
            v => TourExecutionEventsConverter.Read(v)
        )
        .HasColumnType("jsonb");

        modelBuilder.Entity<TourBundle>()
            .HasMany(tb => tb.Tours)
            .WithMany(t => t.TourBundles)
            .UsingEntity<TourTourBundle>();
            
        modelBuilder.Entity<Tour>()
           .Property(item => item.PublishedTours).HasColumnType("jsonb");
        modelBuilder.Entity<Tour>()
           .Property(item => item.ArchivedTours).HasColumnType("jsonb");
        modelBuilder.Entity<Tour>()
           .Property(item => item.TourTimes).HasColumnType("jsonb");
        modelBuilder.Entity<Checkpoint>()
          .Property(item => item.CheckpointSecret).HasColumnType("jsonb");
    }

    private static void ConfigureReportedIssues(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReportedIssue>().HasOne(t => t.Tour).WithMany().HasForeignKey(t => t.TourId);
    }
    private static void ConfigureTourRatings(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<TourRating>().HasOne(t => t.Tour).WithMany().HasForeignKey(t => t.TourId);
        modelBuilder.Entity<Tour>().HasMany(t => t.TourRatings).WithOne(t=>t.Tour).HasForeignKey(t => t.TourId).IsRequired();
    }
}