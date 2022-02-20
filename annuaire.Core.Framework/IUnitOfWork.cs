using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace annuaire.Core.Framework
{
    public interface IUnitOfWork
    {
        int SaveChanges();
    }
}
