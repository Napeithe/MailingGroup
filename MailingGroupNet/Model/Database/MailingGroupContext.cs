using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Entity;

namespace Model.Database
{
    public class MailingGroupContext : IdentityDbContext
    {

        public MailingGroupContext(DbContextOptions<MailingGroupContext> options) :
            base(options)
        {

        }

        public DbSet<MailingGroup> MailingGroups { get; set; }

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
                    .WithOne()
                    .HasForeignKey<MailingGroup>(p => p.UserId);

                x.HasIndex(p => p.Name).IsUnique();
            });

            builder.Entity<Email>(x =>
                x.Property(p => p.Name)
                    .IsRequired());




        }
    }
}
