using annuaire.Core.Employees.Domain;
using annuaire.Core.Employees.Infrastructures.Data;
using annuaire.Core.Employees.Infrastructures.Data.TypeConfigurations;
using annuaire.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annuaire.Core.Employees.Infrastructures.Repositories
{
    public class DefaultJobRepository : IJobRepository
    {
        #region Fields
        private readonly EmployeesContext _context = null;
        #endregion

        #region Constructor
        public DefaultJobRepository(EmployeesContext context)
        {
            this._context = context;
        }
        #endregion

        #region Public Methods
        public IUnitOfWork UnitOfWork => this._context;
        //Post
        public Job AddJob(Job jobToAdd)
        {
            return this._context.Jobs.Add(jobToAdd).Entity;
        }
        //Delete
        public void DeleteJob(Job jobToDelete)
        {
            this._context.Remove(jobToDelete);
        }
        //Get
        public Job FindByJobName(string jobName)
        {
            var jobToFind = this._context.Jobs
                .Where(j => j.JobName == jobName).FirstOrDefault();
            return jobToFind;
        }
        //Get
        public ICollection<Job> GetAllJob()
        {
            return this._context.Jobs.ToList();
        }
        #endregion
    }
}
