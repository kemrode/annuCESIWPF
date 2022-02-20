using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace annuaire.WPF.Controllers
{
    class ErrorController
    {
        #region Properties
        #endregion

        #region Constructor
        public ErrorController() {}
        #endregion

        #region Public Methods

        public void ShowErrorMessage(string message) { MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
        public void ShowSuccessMessage(string message) { MessageBox.Show(message, "Succès", MessageBoxButton.OK, MessageBoxImage.Information); }
        #endregion
    }
}
