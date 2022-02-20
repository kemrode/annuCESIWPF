using annuaire.WPF.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace annuaire.WPF
{
    /// <summary>
    /// Logique d'interaction pour LoginAdmin.xaml
    /// </summary>
    public partial class LoginAdmin : Window
    {
        public bool isAdmin = false;
        private Admin _admin = new Admin("cesiAdmin","root@dminC3si",false);
        public LoginAdmin()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTxt.Text;
            string password = passwordTxt.Text;
            if(login == _admin.AdminName && password == _admin.AdminPassword)
            {
                _admin.IsAdmin = true;
                isAdmin = _admin.IsAdmin;
                MessageBox.Show("Vous êtes maintenant connecté en tant qu'administrateur", "Connexion", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            } else
            {
                MessageBox.Show("Erreur mot de passe/Login", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
