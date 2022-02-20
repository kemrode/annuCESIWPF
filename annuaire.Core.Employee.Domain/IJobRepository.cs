using annuaire.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annuaire.Core.Employees.Domain
{
    public interface IJobRepository : IRepository
    {
        ICollection<Job> GetAllJob();
        void DeleteJob(Job jobToDelete);
        Job AddJob(Job jobToAdd);
        Job FindByJobName(string jobName);
    }
}
