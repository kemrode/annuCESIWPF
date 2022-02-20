using annuaire.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annuaire.Core.Employees.Domain
{
    public interface IPlaceRepository : IRepository
    {
        ICollection<Place> GetallPlace();
        Place AddPlace(Place place);
        Place FindByName(string placeName);
        void DeletePlace(Place placeTodelete);
    }
}
