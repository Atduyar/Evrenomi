
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

        public DbSet<Blog> blogs { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<OperationClaim> operationclaims { get; set; }
        public DbSet<UserOperationClaim> useroperationclaims { get; set; }
        public DbSet<BlogComment> blogcomments { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<BlogTag> blogtags { get; set; }
        public DbSet<BlogEmoji> blogemojis { get; set; }
        public DbSet<BlogEmojiView> blogemoji_views { get; set; }
        public DbSet<BlogEmojiViewView> blogemojiview_views { get; set; }
        public DbSet<BlogEmojiCountView> blogemojicount_views { get; set; }
        public DbSet<UserNotification> usernotifications { get; set; }

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