using CVisionary.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CVisionary.Data;

public class ApplicationDbContext : IdentityDbContext<Person>
{

    public DbSet<Admin> Admins { get; set; }

    public DbSet<EndUser> EndUsers { get; set; }

    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Experience> Experiences { get; set; }



    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Resume - EndUser (many resumes to one EndUser)
        builder.Entity<Resume>()
            .HasOne(r => r.EndUser)
            .WithMany(e => e.Resumes)
            .HasForeignKey("EndUserId")
            .OnDelete(DeleteBehavior.Cascade);

        // Portfolio - EndUser (many portfolios to one EndUser)
        builder.Entity<Portfolio>()
            .HasOne(p => p.EndUser)
            .WithMany(e => e.Portfolios)
            .HasForeignKey("EndUserId")
            .OnDelete(DeleteBehavior.Cascade);

        // Certificate - Resume (many certificates to one resume)
        builder.Entity<Certificate>()
            .HasOne(c => c.Resume)
            .WithMany(r => r.Certificates)
            .HasForeignKey(c => c.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Education - Resume (many educations to one resume)
        builder.Entity<Education>()
            .HasOne(e => e.Resume)
            .WithMany(r => r.Educations)
            .HasForeignKey(e => e.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Experience - Resume (many experiences to one resume)
        builder.Entity<Experience>()
            .HasOne(e => e.Resume)
            .WithMany(r => r.Experiences)
            .HasForeignKey(e => e.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Language - Resume (many languages to one resume)
        builder.Entity<Language>()
            .HasOne(l => l.Resume)
            .WithMany(r => r.Languages)
            .HasForeignKey(l => l.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Skill - Resume (many skills to one resume)
        builder.Entity<Skill>()
            .HasOne(sk => sk.Resume)
            .WithMany(r => r.Skills)
            .HasForeignKey(sk => sk.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Project - Service (many projects to one service)
        builder.Entity<Project>()
            .HasOne(p => p.Service)
            .WithMany(s => s.Projects)
            .HasForeignKey(p => p.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Project - Portfolio (many projects to one portfolio)
        builder.Entity<Project>()
            .HasOne(p => p.Portfolio)
            .WithMany(pf => pf.Projects)
            .HasForeignKey(p => p.PortfolioId)
            .OnDelete(DeleteBehavior.Cascade);

        //   old one-to-many:
        // builder.Entity<Service>()
        //     .HasOne(s => s.Portfolio)
        //     .WithMany(p => p.Services)
        //     .HasForeignKey(s => s.PortfolioId)
        //     .OnDelete(DeleteBehavior.Cascade);

        //  NEW: Many-to-many between Portfolio and Service using PortfolioService

        builder.Entity<PortfolioService>()
            .HasKey(ps => new { ps.PortfolioId, ps.ServiceId });

        builder.Entity<PortfolioService>()
            .HasOne(ps => ps.Portfolio)
            .WithMany(p => p.PortfolioServices)
            .HasForeignKey(ps => ps.PortfolioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PortfolioService>()
            .HasOne(ps => ps.Service)
            .WithMany(s => s.PortfolioServices)
            .HasForeignKey(ps => ps.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Resume - IsDeleted default false
        builder.Entity<Resume>()
            .Property(r => r.IsDeleted)
            .HasDefaultValue(false);

        // Portfolio - IsDeleted default false
        builder.Entity<Portfolio>()
            .Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        // Experience -IsCurrent default false 
        builder.Entity<Experience>()
            .Property(e => e.IsCurrent)
            .HasDefaultValue(false);
    }
}
