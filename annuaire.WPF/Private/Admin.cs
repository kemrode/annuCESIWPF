using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annuaire.WPF.Private
{
    class Admin
    {
        #region Properties
        public string AdminName { get; set; }
        public string AdminPassword { get; set; }
        public bool IsAdmin { get; set; }
        #endregion

        #region Constructor
        public Admin(string _adminName, string _adminPassword,bool _isAdmin)
        {
            AdminName = _adminName;
            AdminPassword = _adminPassword;
            IsAdmin = _isAdmin;
        }
        #endregion
    }
}
