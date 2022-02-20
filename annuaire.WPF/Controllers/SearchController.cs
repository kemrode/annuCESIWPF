using annuaire.WPF.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annuaire.WPF.Controllers
{
    class SearchController
    {
        #region Properties
        private ErrorController _message = new ErrorController();
        private List<Employee> allEmployee = new List<Employee>();
        #endregion

        #region Constructor
        public SearchController() { }
        #endregion

        #region Public Methods

        public async Task<List<Employee>> SearchByName(string Name)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/name/{Name}", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(result.Content).ToList();
                    return allEmployee;
                }
                else
                {
                    allEmployee = SearchByFirstname(Name).GetAwaiter().GetResult();
                    return allEmployee;
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message.ToString());
                return allEmployee;
            }

        }
        private async Task<List<Employee>> SearchByFirstname(string Firstname)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/firstname/{Firstname}", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(result.Content);
                    return allEmployee;
                }
                else
                {
                    allEmployee = SearchByMail(Firstname).GetAwaiter().GetResult();
                    return allEmployee;
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message.ToString());
                return allEmployee;
            }
        }
        private async Task<List<Employee>> SearchByMail(string Mail)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/mail/{Mail}", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Employee employeeFind = JsonConvert.DeserializeObject<Employee>(result.Content);
                    allEmployee = new List<Employee>();
                    allEmployee.Add(employeeFind);
                    return allEmployee;
                }
                else
                {
                    allEmployee = SearchJobByName(Mail).GetAwaiter().GetResult();
                    return allEmployee;
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message.ToString());
                return allEmployee;
            }
        }
        private async Task<List<Employee>> SearchJobByName(string jobName)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/job/{jobName}", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(result.Content);
                    return allEmployee;
                }
                else
                {
                    allEmployee = SearchByPhoneNumber(jobName).GetAwaiter().GetResult();
                    return allEmployee;
                }

            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message);
                return allEmployee;
            }
        }
        private async Task<List<Employee>> SearchByPhoneNumber(string Phone)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/phone/{Phone}", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(result.Content);
                    return allEmployee;
                }
                else
                {
                    allEmployee = SearchByPlacename(Phone).GetAwaiter().GetResult();
                    return allEmployee;
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message.ToString());
                return allEmployee;
            }
        }
        private async Task<List<Employee>> SearchByPlacename(string place)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/{place}", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(result.Content);
                    return allEmployee;
                }
                else
                {
                    _message.ShowErrorMessage("Désolé, votre recherche n'a pu aboutir");
                    return allEmployee;
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message);
                return allEmployee;
            }
        }
        #endregion
    }
}
