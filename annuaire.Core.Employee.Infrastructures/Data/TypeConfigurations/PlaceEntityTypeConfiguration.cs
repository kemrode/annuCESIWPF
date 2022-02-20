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
    class PlaceEntityTypeConfiguration : IEntityTypeConfiguration<Place>
    {
        #region Public Methods
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.ToTable("Place");

            builder.HasKey(item => item.Id);
        }
        #endregion
    }
}
