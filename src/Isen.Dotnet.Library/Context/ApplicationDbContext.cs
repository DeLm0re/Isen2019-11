using System.Diagnostics.CodeAnalysis;
using Isen.Dotnet.Library.Model;
using Microsoft.EntityFrameworkCore;

namespace Isen.Dotnet.Library.Context
{    
    public class ApplicationDbContext : DbContext
    {        
        // Listes des classes modèle / tables
        public DbSet<Person> PersonCollection { get; set; }
        public DbSet<Service> ServiceCollection { get; set; }
        public DbSet<Role> RoleCollection { get; set; }

        public ApplicationDbContext(
            [NotNullAttribute] DbContextOptions options) : 
            base(options) {  }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table de Person
            modelBuilder.Entity<Person>()
                .ToTable(nameof(Person))
                .HasKey(p => p.Id);

            // Table de Role
            modelBuilder.Entity<Role>()
                .ToTable(nameof(Role))
                .HasKey(r => r.Id);

            // Table entre Role et Person
            modelBuilder.Entity<PersonRole>()
                // Clé composite des deux ids
                .HasKey(pr => new { pr.PersonId, pr.RoleId});
            // Relation côté Person
            modelBuilder.Entity<PersonRole>()
                .HasOne(pr => pr.Person)
                .WithMany(p => p.PersonRoles)
                .HasForeignKey(pr => pr.PersonId);
            // Relation côté Role
            modelBuilder.Entity<PersonRole>()
                .HasOne(pr => pr.Role)
                .WithMany(r => r.PersonRoles)
                .HasForeignKey(pr => pr.RoleId);
            
            modelBuilder
                .Entity<Service>()
                .ToTable(nameof(Service))
                .HasKey(s => s.Id);
        }

    }
}