using annuaire.Core.Employees.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annuaire.Core.Employees.Infrastructures.Data.TypeConfigurations
{
    class JobEntityTypeConfiguration : IEntityTypeConfiguration<Job>
    {
        #region Public Methods
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("Job");

            builder.HasKey(item => item.Id);
        }
        #endregion
    }
}
