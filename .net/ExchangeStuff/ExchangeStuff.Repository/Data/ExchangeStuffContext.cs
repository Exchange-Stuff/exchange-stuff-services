using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using ExchangeStuff.CurrentUser.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExchangeStuff.Repository.Data
{
    public partial class ExchangeStuffContext : DbContext
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
            => optionsBuilder.UseSqlServer(GetConnectionString(), x => x.MigrationsAssembly("ExchangeStuff"));

        private string GetConnectionString()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();
            return configurationRoot.GetConnectionString("ExchangeStuffDb") + "";
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
        public DbSet<BanReason> BanReasons { get; set; }
        public DbSet<UserBanReport> UserBanReports { get; set; }
        public DbSet<ProductBanReport> ProductBanReports { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<MessageChat> MessageChats { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GroupChat>(entity =>
            {
                entity.ToTable("GroupChat");

                entity.HasOne(d => d.Receiver)
                .WithMany(p => p.GroupChatReceivers)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(d => d.ReceiverId);

                entity.HasOne(d => d.Sender)
                .WithMany(p => p.GroupChatSenders)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(d => d.SenderId);
            });

            modelBuilder.Entity<MessageChat>(x =>
            {
                x.ToTable("MessageChat");
                x.HasOne(x => x.Sender).WithMany(x => x.MessageChats).HasForeignKey(x => x.SenderId);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Auditable<Guid>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity.CreatedBy == Guid.Empty) entry.Entity.CreatedBy = _identityUser.AccountId;
                        entry.Entity.CreatedOn = DateTime.Now;
                        if (entry.Entity.ModifiedBy == Guid.Empty) entry.Entity.ModifiedBy = _identityUser.AccountId;
                        entry.Entity.ModifiedOn = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        if (entry.Entity.ModifiedBy == Guid.Empty) entry.Entity.ModifiedBy = _identityUser.AccountId;
                        entry.Entity.ModifiedOn = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
