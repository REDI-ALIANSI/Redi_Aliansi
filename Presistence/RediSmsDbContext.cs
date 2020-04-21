using Application.Common.Interfaces;
using Common;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.SMS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presistence
{
    public class RediSmsDbContext : DbContext, IRediSmsDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public RediSmsDbContext(DbContextOptions<RediSmsDbContext> options)
            : base(options)
        {
        }

        public RediSmsDbContext(
            DbContextOptions<RediSmsDbContext> options,
            ICurrentUserService currentUserService,
            IDateTime dateTime)
            : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ServiceCampaign> ServiceCampaigns { get; set; }
        public DbSet<ServiceRenewalConfiguration> ServiceRenewalConfigurations { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<ShortCode> ShortCodes { get; set; }
        public DbSet<Sid> Sids { get; set; }
        public DbSet<SmsdnD> SmsdnDs { get; set; }
        public DbSet<SmsdnHist> SmsdnHists { get; set; }
        public DbSet<SmsinD> SmsinDs { get; set; }
        public DbSet<SmsinHist> SmsinHists { get; set; }
        public DbSet<SmsoutD> SmsoutDs { get; set; }
        public DbSet<SmsoutHist> SmsoutHists { get; set; }
        public DbSet<SubKeyword> SubKeywords { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionHist> SubscriptionHists { get; set; }
        public DbSet<Operator> Operators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RediSmsDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _currentUserService.GetUserId();
                    entry.Entity.Created = _dateTime.Now;

                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedBy = _currentUserService.GetUserId();
                    entry.Entity.LastModified = _dateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
