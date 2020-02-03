using System;
using Microsoft.EntityFrameworkCore;
using Model.Database;

namespace MailingGroupNetTest
{
    public class ContextBuilder
    {
        public static MailingGroupContext BuildClean()
        {
            DbContextOptionsBuilder<MailingGroupContext> dbContextOptionsBuilder =
                new DbContextOptionsBuilder<MailingGroupContext>();

            dbContextOptionsBuilder
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var databaseContext =
                new MailingGroupContext(dbContextOptionsBuilder.Options);
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }
    }
}
