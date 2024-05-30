﻿using ExchangeStuff.Application.Services;
using ExchangeStuff.Core.Common;
using ExchangeStuff.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExchangeStuff.Infrastructure.Persistents
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
            => optionsBuilder.UseSqlServer("");

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
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBalance> UserBalances { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Auditable<Guid>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _identityUser.UserId;
                        entry.Entity.CreatedOn = DateTime.Now;
                        entry.Entity.ModifiedBy = _identityUser.UserId;
                        entry.Entity.ModifiedOn = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _identityUser.UserId;
                        entry.Entity.ModifiedOn = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
