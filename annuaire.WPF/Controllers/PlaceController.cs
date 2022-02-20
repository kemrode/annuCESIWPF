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

namespace annuaire.WPF.Controllers
{
    class PlaceController
    {
        #region Properties
        ErrorController _message = new ErrorController();
        #endregion

        #region Constructor
        public PlaceController() { }
        #endregion

        #region Public Methods

        public async Task<List<Place>> GetAllPlaces()
        {
            List<Place>  placeList = new List<Place>();
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Place/", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    placeList = JsonConvert.DeserializeObject<List<Place>>(result.Content);
                    return placeList;
                }
                else
                {
                    _message.ShowErrorMessage("Erreur lors de la récupération des sites");
                    return placeList;
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message);
                return placeList;
            }

        }
        public async Task<List<Employee>> GetByPlace(string placeName)
        {
            var allEmployee = new List<Employee>();
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Employee/{placeName}", Method.Get);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    allEmployee = JsonConvert.DeserializeObject<List<Employee>>(result.Content);
                    return allEmployee;
                }
                else
                {
                    _message.ShowErrorMessage("Erreur lors de la récupération des données");
                    return allEmployee;
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message);
                return allEmployee;
            }
        }
        public async Task AddPlace(Place newPlace)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest("Place", Method.Post);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddJsonBody(newPlace);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Place newPlaceAdded = JsonConvert.DeserializeObject<Place>(result.Content);
                    _message.ShowSuccessMessage("nouveau site ajouté");                  
                }
                else
                {
                    _message.ShowErrorMessage("Erreur lors de l'ajout du site");
                }
            }
            catch (Exception e)
            {
                _message.ShowErrorMessage(e.Message.ToString());
            }
        }
        public async Task Deleteplace(string placeToDelete)
        {
            try
            {
                var client = new RestClient(ConfigurationManager.ConnectionStrings["AnnuaireAPI"].ConnectionString);
                var request = new RestRequest($"Place/place/{placeToDelete}", Method.Delete);
                var response = client.ExecuteAsync(request);
                var result = response.GetAwaiter().GetResult();
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _message.ShowSuccessMessage($"Le site {placeToDelete} a bien été supprimé");
                }
                else
                {
                    _message.ShowErrorMessage("Erreur lors de la suppression du site");
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
