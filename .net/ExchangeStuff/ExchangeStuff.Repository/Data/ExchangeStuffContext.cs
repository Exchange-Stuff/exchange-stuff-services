using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.CurrentUser.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExchangeStuff.Repository.Data
{
    public class ExchangeStuffContext : DbContext
    {
        private readonly IIdentityUser<Guid> _identityUser;

        public ExchangeStuffContext()
        {

        }
        public ExchangeStuffContext(DbContextOptions<ExchangeStuffContext> options, IIdentityUser<Guid> identityUser) : base(options)
        {
            _identityUser = identityUser;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Data Source=LAPTOP-0DNMTI65;Initial Catalog=ExchangeStuff;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

        private string GetConnectionString()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();
            return configurationRoot["ConnectionStrings:ExchangeStuffDb"] + "";
        }

        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FinancialTicket> FinancialTickets { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<PostTicket> PostTickets { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseTicket> PurchaseTickets { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBalance> UserBalances { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ExchangeStuff.Core.Entities.Action> Actions { get; set; }
        public DbSet<Moderator> Moderators { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Payment> Payments { get; set; }


        /// <summary>
        /// TPH
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<User>()
        //        .ToTable("Users");
        //    modelBuilder.Entity<Admin>()
        //        .ToTable("Admins");
        //    modelBuilder.Entity<Moderator>()
        //      .ToTable("Moderators");
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Auditable<Guid>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _identityUser.AccountId;
                        entry.Entity.CreatedOn = DateTime.Now;
                        entry.Entity.ModifiedBy = _identityUser.AccountId;
                        entry.Entity.ModifiedOn = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _identityUser.AccountId;
                        entry.Entity.ModifiedOn = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
