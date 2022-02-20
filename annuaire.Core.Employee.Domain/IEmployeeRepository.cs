using annuaire.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annuaire.Core.Employees.Domain
{
    public interface IEmployeeRepository : IRepository
    {
        ICollection<Employee> GetAll();
        Employee AddOne(Employee employee);
        ICollection<Employee> GetByPlace(string PlaceName);
        void DeleteById(Employee employeToDelete);
        Employee FindById(int id);
        Employee ChangeEmployee(Employee employee, int id);
        ICollection<Employee> GetbyName(string employeeName);
        ICollection<Employee> GetByFirstname(string employeeFirstname);
        Employee GetByMail(string employeeMail);
        ICollection<Employee> GetByPhonenumber(string phoneNumber);
        ICollection<Employee> GetByJob(string job);
    }
}
