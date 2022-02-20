using annuaire.Core.Employees.Domain;
using annuaire.Core.Employees.Infrastructures.Data.TypeConfigurations;
using annuaire.Core.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annuaire.Core.Employees.Infrastructures.Data
{
    public class EmployeesContext : DbContext, IUnitOfWork
    {
        #region Constructor
        public EmployeesContext([NotNullAttribute] DbContextOptions options) : base(options) { }
        public EmployeesContext() : base() { }
        #endregion

        #region Internal Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlaceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new JobEntityTypeConfiguration());
        }
        #endregion

        #region Properties
        public DbSet<Employee> Employees { get; set; } 
        public DbSet<Place> Places { get; set; }
        public DbSet<Job> Jobs { get; set; }
        #endregion
    }
}
