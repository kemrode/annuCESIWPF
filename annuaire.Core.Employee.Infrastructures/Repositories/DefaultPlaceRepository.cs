using annuaire.Core.Employees.Domain;
using annuaire.Core.Employees.Infrastructures.Data;
using annuaire.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annuaire.Core.Employees.Infrastructures.Repositories
{
    public class DefaultPlaceRepository : IPlaceRepository
    {
        #region Fields
        private readonly EmployeesContext _context = null;
        #endregion

        #region Constructor
        public DefaultPlaceRepository(EmployeesContext context)
        {
            this._context = context;
        }
        #endregion

        #region Properties
        public IUnitOfWork UnitOfWork => this._context;
        #endregion

        #region Publc Methods
        //Post
        public Place AddPlace(Place place)
        {
            return this._context.Places.Add(place).Entity;
        }
        //Delete
        public void DeletePlace(Place placeToDelete)
        {
            this._context.Remove(placeToDelete);
        }
        //Get
        public Place FindByName(string placeName)
        {
            var placeToFind = this._context.Places
                .Where(p => p.PlaceName == placeName).FirstOrDefault();
            return placeToFind;
        }
        //Get
        public ICollection<Place> GetallPlace()
        {
            return this._context.Places.ToList();
        }
        #endregion

    }
}
