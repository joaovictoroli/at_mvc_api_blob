using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;

namespace SocialNetwork.Infra.Context
{
    public class SocialNetworkDbContext : DbContext
    {
        private const string connectionString = "Server=DESKTOP-57P62L3;Database=PB_ASPNET;TrustServerCertificate=True;Trusted_Connection=True;";
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserUsers> UserUsers { get; set; }

        //public SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserUsers>()
                .HasKey(ff => new { ff.UserId, ff.User2Id });

            modelBuilder.Entity<UserUsers>()
                .HasOne(p => p.User)
                .WithMany(u => u.UserContacts)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserUsers>()
                .HasOne(p => p.User2)
                .WithMany(u => u.User2Contacts)
                .HasForeignKey(u => u.User2Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserDetail>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.UserDetail)
                .HasForeignKey(u => u.UserId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
