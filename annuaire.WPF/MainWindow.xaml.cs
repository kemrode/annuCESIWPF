using annuaire.WPF.Controllers;
using annuaire.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace annuaire.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        private bool _isAdmin = false;
        private List<Employee> allEmployee = new List<Employee>();
        private List<Place> _placeList = new List<Place>();
        private List<Job> _jobList = new List<Job>();
        private Employee _employeeToSee = new Employee();
        private List<Key> KonamiSequence = new List<Key> { Key.Up, Key.Up, Key.Down, Key.Down, Key.Left, Key.Right, Key.Left, Key.Right, Key.B, Key.A};
        private List<Key> readSequence = new List<Key>();
        private int keyPosition = 0;
        private EmployeeController _employeeController = new EmployeeController();
        private JobController _jobController = new JobController();
        private PlaceController _placeController = new PlaceController();
        private SearchController _searchController = new SearchController();
        private ErrorController _message = new ErrorController();
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            GetAllEmployee();
            _placeList = _placeController.GetAllPlaces().GetAwaiter().GetResult();
            _jobList = _jobController.GetAllJobs().GetAwaiter().GetResult();
            GetItems();
            bool checkAdmin = CheckIsAdmin();
            if (checkAdmin == false)
            {
                HideAdminElements();
            } else
            {
                ShowAdminElements();
            }
        }
        #endregion

        #region Privates Methods

        public void GetAllEmployee()
        {
            allEmployee = _employeeController.GetAll().GetAwaiter().GetResult();
            DrawStackpanel();
        }

        #region Visual Elements
        private void DrawStackpanel()
        {
            foreach (Employee employee in allEmployee)
            {
                var newEmployeeStackPanel = new StackPanel();
                newEmployeeStackPanel.Orientation = Orientation.Horizontal;
                //Name
                Label nameLabel = DrawLabel(employee.Name);
                newEmployeeStackPanel.Children.Add(nameLabel);
                //Firstname
                Label firstnameLabel = DrawLabel(employee.Firstname);
                newEmployeeStackPanel.Children.Add(firstnameLabel);
                //Job
                Label jobLabel = DrawLabel(employee.Job);
                newEmployeeStackPanel.Children.Add(jobLabel);
                //Place
                Label placeLabel = DrawLabel(employee.Place);
                newEmployeeStackPanel.Children.Add(placeLabel);
                newEmployeeStackPanel.MouseLeftButtonUp += (s, e) =>
                {
                    NameBox.Text = employee.Name;
                    FirstnameBox.Text = employee.Firstname;
                    jobBox.Text = employee.Job;
                    PhoneBox.Text = employee.PhoneNumber.ToString();
                    MailBox.Text = employee.Mail;
                    SiteBox.Text = employee.Place;
                    _employeeToSee = employee;
                };
                //Add all inside StackPanel
                GetAllPanel.Children.Add(newEmployeeStackPanel);
            };
        }
        private Label DrawLabel(string labelMsg)
        {
            var newLabel = new Label();
            newLabel.Content = labelMsg;
            newLabel.Foreground = new SolidColorBrush(Colors.White);
            newLabel.FontSize = 20;
            newLabel.FontWeight = FontWeights.Bold;
            return newLabel;
        }
        private void CreateListOfItems(ComboBox _comboBox)
        {
            foreach (Place item in _placeList)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = item.PlaceName;
                _comboBox.Items.Add(newItem);
            }
        }
        private void GetItems()
        {
            CreateListOfItems(placeList);
            CreateListOfItems(placeDeleting);
            CreateListOfItems(SiteBox);
            JobItems(jobDeleting);
            JobItems(jobBox);
        }
        private void JobItems(ComboBox _comboBox)
        {
            if (_jobList.Count == 0)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = "Aucun service enregistré";
                _comboBox.Items.Add(newItem);

            }
            else
            {
                foreach (Job item in _jobList)
                {
                    ComboBoxItem newItem = new ComboBoxItem();
                    newItem.Content = item.JobName;
                    _comboBox.Items.Add(newItem);
                }
            }
        }
        #endregion

        #region Buttons Events
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Voulez-vous supprimer {_employeeToSee.Name} ?", "suppression", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            _employeeController.DeleteEmployee(_employeeToSee).GetAwaiter().GetResult();
            GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
            GetAllEmployee();
        }
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            var employeeChanged = NewEmployee();
            _employeeController.UpdateEmployee(employeeChanged, _employeeToSee).GetAwaiter().GetResult();
            GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
            GetAllEmployee();
        }
        private void newJobBtn_Click(object sender, RoutedEventArgs e)
        {
            Job _newJob = new Job()
            {
                JobName = newJob.Text,
            };
            bool isInDataBase = CheckIfJobAlreadyInDataBase(_newJob);
            if (isInDataBase)
            {
                newJob.Clear();
                _message.ShowErrorMessage("Donnée déjà enregistrée");
            }
            else
            {
                jobDeleting.Items.Clear();
                jobBox.Items.Clear();
                _jobController.AddJob(_newJob).GetAwaiter().GetResult();
                _jobList = _jobController.GetAllJobs().GetAwaiter().GetResult();
                JobItems(jobDeleting);
                JobItems(jobBox);
                newJob.Clear();
            }
        }
        private void deleteJobBtn_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem job = (ComboBoxItem)jobDeleting.SelectedValue;
            string jobToDelete = job.Content.ToString();
            bool isCountZero = CheckIfJobAtzero(jobToDelete);
            if (jobToDelete != null && isCountZero == true)
            {
                jobDeleting.Items.Clear();
                jobBox.Items.Clear();
                _jobController.DeleteJob(jobToDelete).GetAwaiter().GetResult();
                _jobList = _jobController.GetAllJobs().GetAwaiter().GetResult();
                JobItems(jobDeleting);
                JobItems(jobBox);
            }
            else
            {
                _message.ShowErrorMessage("Désolé, vous ne pouvez pas effectuer cette action");
            }
        }
        private void newSiteBtn_Click(object sender, RoutedEventArgs e)
        {
            Place newPlace = new Place()
            {
                PlaceName = newSite.Text,
            };
            bool checkedDatBase = CheckIfAlreadyInDataBase(newPlace);
            if (checkedDatBase)
            {
                newSite.Clear();
                _message.ShowErrorMessage("Donnée déjà existante");
            }
            else
            {
                _placeList.Clear();
                newSite.Clear();
                _placeController.AddPlace(newPlace).GetAwaiter().GetResult();
                _placeList = _placeController.GetAllPlaces().GetAwaiter().GetResult();
                CreateListOfItems(placeList);
                CreateListOfItems(placeDeleting);
            }
        }
        private void deleteSiteBtn_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem place = (ComboBoxItem)placeDeleting.SelectedItem;
            string placeToDelete = place.Content.ToString();
            bool isCountZero = CheckIfCountZero(placeToDelete);
            if (placeToDelete != "Tous les sites" && isCountZero == true)
            {
                _placeList.Clear();
                _placeController.Deleteplace(placeToDelete).GetAwaiter().GetResult();
                _placeList = _placeController.GetAllPlaces().GetAwaiter().GetResult();
                CreateListOfItems(placeList);
                CreateListOfItems(placeDeleting);
            }
            else
            {
                _message.ShowErrorMessage("Désolé, vous ne pouvez pas effectuer cette action");
            }
        }
        #endregion

        #region AddNewEmployee
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Employee newEmployee = NewEmployee();
            GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
            _employeeController.AddNewEmployee(newEmployee).GetAwaiter().GetResult();
            GetAllEmployee();
        }
        private Employee NewEmployee()
        {
            ComboBoxItem placeSelected = (ComboBoxItem)SiteBox.SelectedItem;
            string placeSelectedName = placeSelected.Content.ToString();
            ComboBoxItem jobSelected = (ComboBoxItem)jobBox.SelectedItem;
            string jobSelectedName = jobSelected.Content.ToString();
            Employee newEmployee = new Employee()
            {
                Name = NameBox.Text,
                Firstname = FirstnameBox.Text,
                Job = jobSelectedName,
                PhoneNumber = PhoneBox.Text,
                Mail = MailBox.Text,
                Place = placeSelectedName,
            };
            return newEmployee;
        }
        #endregion

        #region ComboBox
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem placeSelected = (ComboBoxItem)placeList.SelectedItem;
            var placeName = placeSelected.Content.ToString();
            GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
            if (placeName != "Tous les sites")
            {
                GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
                allEmployee = _placeController.GetByPlace(placeName).GetAwaiter().GetResult();
                DrawStackpanel();
            } else
            {
                GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
                GetAllEmployee();
                DrawStackpanel();
            }
        }
        #endregion

        #region CheckIsAdmin Methods
        private bool CheckIsAdmin()
        {
            return _isAdmin;
        }
        private void HideAdminElements()
        {
            NameBox.IsReadOnly = true;
            FirstnameBox.IsReadOnly = true;
            MailBox.IsReadOnly = true;
            PhoneBox.IsReadOnly = true;
            jobBox.IsReadOnly = true;
            MailBox.IsReadOnly = true;
            SiteBox.IsReadOnly = true;
            searchButton.Visibility = Visibility.Hidden;
            updateButton.Visibility = Visibility.Hidden;
            deleteButton.Visibility = Visibility.Hidden;
        }
        public void ShowAdminElements()
        {
            NameBox.IsReadOnly = false;
            FirstnameBox.IsReadOnly = false;
            MailBox.IsReadOnly = false;
            PhoneBox.IsReadOnly = false;
            jobBox.IsReadOnly = false;
            MailBox.IsReadOnly = false;
            searchButton.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Visible;
            deleteButton.Visibility = Visibility.Visible;
        }
        private void ShowLoginAdmin()
        {
            LoginAdmin showLoginView = new LoginAdmin();
            if(showLoginView.ShowDialog() == true)
            {
            } else
            {
                bool testLogin = showLoginView.isAdmin;
                if (testLogin == true)
                {
                    ShowAdminElements();
                }
            }
        }
        #endregion

        #region Konami Methods
        private void Window_KeyUp(object sender, KeyEventArgs e)
        
        {
            if (_isAdmin == false)
            {                
                IsCompletedBy(e.Key);
            }
        }

        private bool IsCompletedBy(Key key)
        {
            if(key == KonamiSequence.ElementAt(keyPosition))
            {
                keyPosition++;
                readSequence.Add(key);
                if (readSequence.SequenceEqual(KonamiSequence))
                {
                    _isAdmin = true;
                    ShowLoginAdmin();
                }
                return _isAdmin;
            } else
            {
                if(readSequence != null)
                {
                    readSequence = new List<Key>();
                }
                _isAdmin = false;
                return _isAdmin;
            }
        }

        #endregion

        #region Search Methods
        private void searchBar_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                string nameToFind = searchBar.Text;
                if(nameToFind != "rechercher" && nameToFind != null &&
                    nameToFind != "")
                {
                    GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
                    allEmployee = _searchController.SearchByName(nameToFind).GetAwaiter().GetResult();
                    DrawStackpanel();
                } else
                {
                    GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
                    GetAllEmployee();
                }
            }
        }
        #endregion

        #region Clear textbox text methods
        private void searchBar_GotFocus(object sender, RoutedEventArgs e)
        {
            searchBar.Text = string.Empty;
        }
        private void NameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(NameBox.Text == "Nom")
            {
                NameBox.Text = string.Empty;
            }
        }
        private void FirstnameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(FirstnameBox.Text == "Prénom")
            {
                FirstnameBox.Text = string.Empty;
            }
        }
        private void JobBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(jobBox.Text == "Poste")
            {
                jobBox.Text = string.Empty;
            }
        }
        private void PhoneBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(PhoneBox.Text == "Téléphone")
            {
                PhoneBox.Text = string.Empty;
            }
        }
        private void MailBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(MailBox.Text == "Courriel")
            {
                MailBox.Text = string.Empty;
            }
        }
        private void SiteBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(SiteBox.Text == "Site")
            {
                SiteBox.Text = string.Empty;
            }
        }
        #endregion

        #region Checks
        private bool CheckIfAlreadyInDataBase(Place newPlace)
        {
            foreach (Place place in _placeList)
            {
                if (place.PlaceName == newPlace.PlaceName)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckIfCountZero(string placeName)
        {
            GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
            allEmployee = _placeController.GetByPlace(placeName).GetAwaiter().GetResult();
            if (allEmployee.Count == 0)
            {
                return true;
            }
            return false;
        }
        private bool CheckIfJobAlreadyInDataBase(Job jobName)
        {
            foreach (Job job in _jobList)
            {
                if (job.JobName == jobName.JobName)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckIfJobAtzero(string jobName)
        {
            allEmployee = _jobController.GetByJobNameEmployees(jobName).GetAwaiter().GetResult();
            if (allEmployee.Count == 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #endregion
    }
}
