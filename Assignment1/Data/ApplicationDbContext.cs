using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ForumApp.Models;

namespace ForumApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity tables are included

            modelBuilder.Entity<Discussion>()
    .HasOne(d => d.User)
    .WithMany()
    .HasForeignKey(d => d.UserId)
    .HasPrincipalKey(u => u.Id)
    .HasConstraintName("FK_Discussions_AspNetUsers_UserId");

            modelBuilder.Entity<IdentityUser>()
                .ToTable("AspNetUsers"); // Ensure IdentityUser maps to AspNetUsers

        }

    }
}
