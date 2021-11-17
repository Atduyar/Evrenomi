
using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class VikingContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(@"Server=localhost; Port=3306; User=rootApi; Password=123654APIroot!; Database=myapi");
            //optionsBuilder.UseMySql(@"Server=localhost; Port=3306; User=root; Password=toor; Database=vikingtest");
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;initial catalog=Northwind;integrated security=true");
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<BlogEmoji> Blogemojis { get; set; }
        public DbSet<BlogEmojiView> Blogemoji_views { get; set; }
        public DbSet<BlogEmojiViewView> Blogemojiview_views { get; set; }
        public DbSet<BlogEmojiCountView> Blogemojicount_views { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        //public DbSet<User> Users { get; set; }
        //public DbSet<OperationClaim> OperationClaims { get; set; }
        //public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

        //public DbSet<Product> Products { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<User> Users { get; set; }
        //public DbSet<OperationClaim> OperationClaims { get; set; }
        //public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    }
}