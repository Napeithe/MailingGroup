using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Entity;

namespace Model.Database
{
    public class MailingGroupContext : IdentityDbContext<AppUser>
    {

        public MailingGroupContext(DbContextOptions<MailingGroupContext> options) :
            base(options)
        {

        }

        public DbSet<MailingGroup> MailingGroups { get; set; }
        public DbSet<Email> Emails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MailingGroup>(x =>
            {
                x.Property(p => p.Name)
                    .IsRequired();

                x.HasMany(p => p.Emails)
                    .WithOne()
                    .HasForeignKey(p => p.MailingGroupId);

                x.HasOne(p => p.User)
                    .WithMany(p=>p.MailingGroups)
                    .HasForeignKey(p=>p.UserId)
                    .IsRequired();
            });

            builder.Entity<Email>(x =>
            {
                x.Property(p => p.Name)
                    .IsRequired();
                x.HasOne(p => p.MailingGroup)
                    .WithMany(p => p.Emails)
                    .HasForeignKey(p => p.MailingGroupId)
                    .IsRequired();
            });




        }
    }
}
