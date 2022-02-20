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
    class JobController
    {
        #region Properties
        private ErrorController _message = new ErrorController();
        #endregion

        #region Constructor
        public JobController() { }
        #endregion

        #region Public Methods

        public async Task<List<Job>> GetAllJobs()
        {
            List<Job> jobList = new List<Job>();
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Job", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    jobList = new List<Job>();
                    jobList = JsonConvert.DeserializeObject<List<Job>>(result.Content);
                    return jobList;
                }
                else
                {
                    _message.ShowErrorMessage("Erreur lors de la récupération des données");
                    return jobList;
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message.ToString());
                return jobList;
            }
        }
        public async Task<List<Employee>> GetByJobNameEmployees(string jobName)
        {
            List<Employee> allEmployee = new List<Employee>();
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
                    _message.ShowErrorMessage("Aucune correspondance");
                    return allEmployee;
                }

            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message);
                return allEmployee;
            }
        }
        public async Task AddJob(Job _newJob)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Job", Method.Post);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddJsonBody(_newJob);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Job jobAdded = JsonConvert.DeserializeObject<Job>(result.Content);
                    _message.ShowSuccessMessage("Enregistrement d'un nouveau service effectué");
                }
                else
                {
                    _message.ShowErrorMessage("Erreur lors de l'enregistrement de la donnée");
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message.ToString());
            }
        }
        public async Task DeleteJob(string jobToDelete)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Job/job/{jobToDelete}", Method.Delete);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _message.ShowSuccessMessage($"le service {jobToDelete} a bien été supprimé");
                }
                else
                {
                    _message.ShowErrorMessage("Erreur lors de la suppression de la donnée");
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message.ToString());
            }
        }
        #endregion
    }
}
