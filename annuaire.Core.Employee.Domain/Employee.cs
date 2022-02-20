using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace annuaire.Core.Employees.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Job { get; set; }
        public string PhoneNumber { get; set; }
        public string Place { get; set; }
        public string Mail { get; set; }
    }
}
