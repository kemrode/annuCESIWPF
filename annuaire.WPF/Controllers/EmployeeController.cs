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

namespace annuaire.WPF.Controllers
{
    class EmployeeController
    {
        #region Properties
        private ErrorController _message = new ErrorController();
        #endregion

        #region Constructor
        public EmployeeController() {}
        #endregion

        #region Public Methods

        public async Task<List<Employee>> GetAll()
        {
            List<Employee> allEmployee = new List<Employee>();
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Employee", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();

                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(result.Content);
                    return allEmployee;
                }
                else
                {
                        _message.ShowErrorMessage("Erreur de la récupération des données");
                    return allEmployee;
                }
            }
            catch (Exception e)
            {
                    _message.ShowErrorMessage(e.Message.ToString());
                return allEmployee;
            }
        }
        public async Task AddNewEmployee(Employee employee)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Employee", Method.Post);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddJsonBody(employee);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Employee newEmployeeAdded = JsonConvert.DeserializeObject<Employee>(result.Content);
                    _message.ShowSuccessMessage("Nouvel employé enregistré");
                }
                else
                {
                    _message.ShowErrorMessage("Erreur lors de la récupération des données");
                }

            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message.ToString());
            }
        }
        public async Task DeleteEmployee(Employee employee)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/{employee.Id}", Method.Delete);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show($"L'employé {employee.Name} supprimé", "suppression", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show($"L'utilisateur {employee.Name} n'a pas pu être supprimé", "suppression", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message.ToString());
            }

        }
        public async Task UpdateEmployee(Employee employeeUpdate, Employee employeeToSee)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/{employeeToSee.Id}", Method.Put);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddJsonBody(employeeUpdate);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Modifications effectuées", "Modifications", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Console.WriteLine(response.Result.StatusCode);
                    MessageBox.Show("Erreur modifications impossibles", "Modifications", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur modifications impossibles", "Modifications", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

    }

}
