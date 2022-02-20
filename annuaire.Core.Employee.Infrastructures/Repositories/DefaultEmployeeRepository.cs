using annuaire.Core.Employees.Domain;
using annuaire.Core.Employees.Infrastructures.Data;
using annuaire.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annuaire.Core.Employees.Infrastructures.Repositories
{
    public class DefaultEmployeeRepository : IEmployeeRepository
    {
        #region Fields
        private readonly EmployeesContext _context = null;
        #endregion
        #region Constructor
        public DefaultEmployeeRepository(EmployeesContext context)
        {
            this._context = context;
        }

        #endregion
        #region Public Methods
        public ICollection<Employee> GetAll()
        {
            return this._context.Employees.ToList();
        }

        public Employee AddOne(Employee employee)
        {
            return this._context.Employees.Add(employee).Entity;
        }

        public ICollection<Employee> GetByPlace(string PlaceName)
        {
            var employeeListPlace = this._context.Employees
                .Where(e => e.Place == PlaceName).ToList();
            return employeeListPlace;
        }

        public void DeleteById(Employee employeeToDelete)
        {
            this._context.Remove(employeeToDelete);
        }

        public Employee FindById(int id)
        {
            var employeeToFind = this._context.Employees
                .Where(e => e.Id == id).FirstOrDefault();
            return employeeToFind;
        }

        public Employee ChangeEmployee(Employee employee, int id)
        {
            Employee employeeToChange = this._context.Employees
                .Where(e => e.Id == id).FirstOrDefault();

            employeeToChange.Name = employee.Name;
            employeeToChange.Firstname = employee.Firstname;
            employeeToChange.Job = employee.Job;
            employeeToChange.Mail = employee.Mail;
            employeeToChange.PhoneNumber = employee.PhoneNumber;
            employeeToChange.Place = employee.Place;

            this._context.Entry(employeeToChange).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return employeeToChange;
                
        }

        public ICollection<Employee> GetbyName(string employeeName)
        {
            List<Employee> employeeToFind = this._context.Employees
                .Where(e => e.Name == employeeName).ToList();
            return employeeToFind;
        }

        public ICollection<Employee> GetByFirstname(string employeeFirstname)
        {
            List<Employee> employeeToFind = this._context.Employees
                .Where(e => e.Firstname == employeeFirstname).ToList();
            return employeeToFind;
        }

        public Employee GetByMail(string employeeMail)
        {
            Employee employeeToFind = this._context.Employees
                .Where(e => e.Mail == employeeMail).FirstOrDefault();
            return employeeToFind;
        }

        public ICollection<Employee> GetByPhonenumber(string phoneNumber)
        {
            List<Employee> phonenumberToFind = this._context.Employees
                .Where(e => e.PhoneNumber == phoneNumber).ToList();
            return phonenumberToFind;
        }

        public ICollection<Employee> GetByJob(string job)
        {
            List<Employee> jobToFind = this._context.Employees
                .Where(e => e.Job == job).ToList();
            return jobToFind;
        }

        #endregion

        #region Properties
        public IUnitOfWork UnitOfWork => this._context;
        #endregion
    }
}
