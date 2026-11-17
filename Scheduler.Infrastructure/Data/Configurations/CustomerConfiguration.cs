using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduler.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Infrastructure.Data.Configurations
{
    internal class CustomerConfiguration
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.OwnsOne(c => c.Address)
        }
    }
}
