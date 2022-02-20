using annuaire.WPF.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            GetAllEmployee();
            GetPlaces();
            GetJobs();
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

        #region GetAllEmployee
        private void GetPlaces()
        {
            GetAllPlaces().GetAwaiter().GetResult();
        }
        private void GetAllEmployee()
        {
            GetAll().GetAwaiter().GetResult();
            DrawStackpanel();
        }
        private async Task GetAll()
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Employee/", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();

                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    allEmployee = new List<Employee>();
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(result.Content);
                }
                else
                {
                    Console.WriteLine("error");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
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
        #endregion

        #region ButtonClick_AddNewEmployee
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Employee newEmployee = NewEmployee();
            GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
            Task task = AddNewEmployee(newEmployee);
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
        private async Task AddNewEmployee(Employee employee)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Employee", Method.Post);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddJsonBody(employee);
                var response =
                    await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    Employee newEmployeeAdded = JsonConvert.DeserializeObject<Employee>(response.Content);
                    MessageBox.Show("Nouvel employé enregistré", "Enregistrement", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    GetAllEmployee();
                } else
                {
                    Console.WriteLine(response.StatusCode);
                }

            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region ButtonClick_DeleteEmployee
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Voulez-vous supprimer {_employeeToSee.Name} ?", "suppression", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            Task task = DeleteEmployee(_employeeToSee);
        }
        private async Task DeleteEmployee(Employee employee)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/{employee.Id}", Method.Delete);
                var response =
                    await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    MessageBox.Show($"L'employé {employee.Name} supprimé", "suppression", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
                    GetAllEmployee();
                } else
                {
                    Console.WriteLine(response.StatusCode);
                    MessageBox.Show($"L'utilisateur {employee.Name} n'a pas pu être supprimé", "suppression", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

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
                GetByPlace(placeName).GetAwaiter().GetResult();
                DrawStackpanel();
            } else
            {
                GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
                GetAllEmployee();
                DrawStackpanel();
            }
        }
        private void CreateListOfItems(ComboBox _comboBox)
        {
            foreach(Place item in _placeList)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = item.PlaceName;
                _comboBox.Items.Add(newItem);
            }
        }
        private void ClearComboxes()
        {
            placeList.Items.Clear();
            placeDeleting.Items.Clear();
            SiteBox.Items.Clear();
        }
        private void GetItems()
        {
            CreateListOfItems(placeList);
            CreateListOfItems(placeDeleting);
            CreateListOfItems(SiteBox);
            JobItems(jobDeleting);
            JobItems(jobBox);
        }
        #endregion

        #region Update Method
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            var employeeChanged = NewEmployee();
            UpdateEmployee(employeeChanged).GetAwaiter().GetResult();
            GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
            GetAllEmployee();
        }

        private async Task UpdateEmployee(Employee employeeUpdate)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/{_employeeToSee.Id}", Method.Put);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddJsonBody(employeeUpdate);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Modifications effectuées", "Modifications", MessageBoxButton.OK, MessageBoxImage.Information);
                } else
                {
                    Console.WriteLine(response.Result.StatusCode);
                    MessageBox.Show("Erreur modifications impossibles", "Modifications", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            } catch (Exception e)
            {
                MessageBox.Show("Erreur modifications impossibles", "Modifications", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    Task task = SearchByName(nameToFind);
                    DrawStackpanel();
                } else
                {
                    GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
                    GetAllEmployee();
                }
            }

        }
        private async Task SearchByName(string Name)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/name/{Name}", Method.Get);
                var response = 
                    await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(response.Content).ToList();
                }
                else
                {
                    Task task = SearchByFirstname(Name);
                }

            } catch (Exception e)
            {
                ShowErrorMessage(e.Message.ToString());
            }

        }
        private async Task SearchByFirstname(string Firstname)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/firstname/{Firstname}", Method.Get);
                var response =
                    await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
                }
                else
                {
                    Task task = SearchByMail(Firstname);
                }
            } catch (Exception e)
            {
                ShowErrorMessage(e.Message.ToString());
            }
        }
        private async Task SearchByMail(string Mail)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/mail/{Mail}", Method.Get);
                var response = 
                    await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    Employee employeeFind = JsonConvert.DeserializeObject<Employee>(response.Content);
                    allEmployee = new List<Employee>();
                    allEmployee.Add(employeeFind);
                    DrawTextBox(employeeFind);
                }
                else
                {
                    Task task = SearchJobByName(Mail);
                }
            } catch (Exception e)
            {
                ShowErrorMessage(e.Message.ToString());
            }
        }
        private async Task SearchByPhoneNumber(string Phone)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/phone/{Phone}", Method.Get);
                var response = 
                    await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
                }
                else
                {
                    ShowErrorMessage("Désolé, votre recherche n'a pu aboutir");
                    GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
                }
            } catch(Exception e)
            {
                ShowErrorMessage(e.Message);
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

        #region ComboBox Places & Jobs Methods

        #region Places Methods
        private async Task GetByPlace(string placeName)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/{placeName}", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(result.Content);
                }
                else
                {
                    ShowErrorMessage("Erreur lors de la récupération des données");
                    GetAllPanel.Children.RemoveRange(0, allEmployee.Count());
                }
            } catch (Exception e)
            {
                ShowErrorMessage(e.Message);
            }
        }
        private async Task GetAllPlaces()
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Place/", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _placeList = new List<Place>();         
                    _placeList = JsonConvert.DeserializeObject<List<Place>>(result.Content);
                } else
                {
                    ShowErrorMessage("Erreur lors de la récupération des sites");
                }
            } catch (Exception e)
            {
                ShowErrorMessage(e.Message);
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
                ShowErrorMessage("Donnée déjà existante");
            } else
            {
                ClearComboxes();
                Task task = AddPlace(newPlace);
            }
        }
        private async Task AddPlace(Place newPlace)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Place", Method.Post);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddJsonBody(newPlace);
                var response =
                    await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    Place newPlaceAdded = JsonConvert.DeserializeObject<Place>(response.Content);
                    MessageBox.Show("nouveau site ajouté", "Ajout", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    newSite.Clear();
                    GetAllPlaces().GetAwaiter().GetResult();
                    GetItems();
                } else
                {
                    MessageBox.Show("Erreur lors de l'ajout du site", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void deleteSiteBtn_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem place = (ComboBoxItem)placeDeleting.SelectedItem;
            string placeToDelete = place.Content.ToString();
            bool isCountZero = CheckIfCountZero(placeToDelete);
            if(placeToDelete != "Tous les sites" && isCountZero == true)
            {
                ClearComboxes();
                Task task = Deleteplace(placeToDelete);
            } else
            {
                ShowErrorMessage("Désolé, vous ne pouvez pas effectuer cette action");
            }
        }
        private async Task Deleteplace(string placeToDelete)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Place/place/{placeToDelete}", Method.Delete);
                var response =
                    await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    ShowSuccessMessage($"Le site {placeToDelete} a bien été supprimé");
                    GetAllPlaces().GetAwaiter().GetResult();
                    GetItems();
                } else
                {
                    ShowErrorMessage("Erreur lors de la suppression du site");
                }

            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
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
            GetByPlace(placeName).GetAwaiter().GetResult();
            if(allEmployee.Count == 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Jobs Methods
        private void GetJobs()
        {
            GetAllJobs().GetAwaiter().GetResult();
        }
        private async Task GetAllJobs()
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Job", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _jobList = new List<Job>();
                    _jobList = JsonConvert.DeserializeObject<List<Job>>(result.Content);
                } else
                {
                    ShowErrorMessage("Erreur lors de la récupération des données");
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ShowErrorMessage(e.Message.ToString());
            }
        }
        private void JobItems(ComboBox _comboBox)
        {
            if(_jobList.Count == 0)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = "Aucun service enregistré";
                _comboBox.Items.Add(newItem);
                    
            } else
            {
                foreach (Job item in _jobList)
                {
                    ComboBoxItem newItem = new ComboBoxItem();
                    newItem.Content = item.JobName;
                    _comboBox.Items.Add(newItem);
                }
            }
        }
        private async Task SearchJobByName(string jobName)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/job/{jobName}", Method.Get);
                var response =
                    await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
                }else
                {
                    Task task = SearchByPhoneNumber(jobName);
                }

            }catch(Exception e)
            {
                ShowErrorMessage(e.Message);
            }
        }
        private async Task GetByJobNameEmployees(string jobName)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/job/{jobName}", Method.Get);
                var response  = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(result.Content);
                }
                else
                {
                    allEmployee = new List<Employee>();
                }

            }
            catch (Exception e)
            {
                ShowErrorMessage(e.Message);
            }
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
                ShowErrorMessage("Donnée déjà enregistrée");
            } else
            {
                jobDeleting.Items.Clear();
                Task task = AddJob(_newJob);
            }
        }
        private async Task AddJob(Job _newJob)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Job", Method.Post);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddJsonBody(_newJob);
                var response =
                    await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    Job jobAdded = JsonConvert.DeserializeObject<Job>(response.Content);
                    jobDeleting.Items.Clear();
                    jobBox.Items.Clear();
                    GetJobs();
                    JobItems(jobDeleting);
                    JobItems(jobBox);
                    ShowSuccessMessage("Enregistrement d'un nouveau service effectué");
                    newJob.Clear();
                } else
                {
                    ShowErrorMessage("Erreur lors de l'enregistrement de la donnée");
                }
            } catch (Exception e)
            {
                ShowErrorMessage(e.Message.ToString());
            }
        }
        private void deleteJobBtn_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem job = (ComboBoxItem)jobDeleting.SelectedValue;
            string jobToDelete = job.Content.ToString();
            bool isCountZero = CheckIfJobAtzero(jobToDelete);
            if(jobToDelete != null && isCountZero == true)
            {
                jobDeleting.Items.Clear();
                jobBox.Items.Clear();
                Task task = DeleteJob(jobToDelete);
            } else
            {
                ShowErrorMessage("Désolé, vous ne pouvez pas effectuer cette action");
            }
        }
        private async Task DeleteJob(string jobToDelete)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Job/job/{jobToDelete}", Method.Delete);
                var response =
                    await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    ShowSuccessMessage($"le service {jobToDelete} a bien été supprimé");
                    GetJobs();
                    JobItems(jobDeleting);
                    JobItems(jobBox);
                } else
                {
                    ShowErrorMessage("Erreur lors de la suppression de la donnée");
                }

            } catch (Exception e)
            {
                ShowErrorMessage(e.Message.ToString());
            }
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
            GetByJobNameEmployees(jobName).GetAwaiter().GetResult();
            if (allEmployee.Count == 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #endregion

        private void DrawTextBox(Employee employee)
        {
            NameBox.Text = employee.Name;
            FirstnameBox.Text = employee.Firstname;
            jobBox.Text = employee.Job;
            SiteBox.Text = employee.Place;
            PhoneBox.Text = employee.PhoneNumber;
            MailBox.Text = employee.Mail;
        }

        #endregion

        #region Messages Methods
        private void ShowErrorMessage(string message) { MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
        private void ShowSuccessMessage(string message) { MessageBox.Show(message, "Succès", MessageBoxButton.OK, MessageBoxImage.Information); }

        #endregion

    }
}
