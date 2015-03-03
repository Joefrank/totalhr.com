using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface IAbsencesRepository : IGenericRepository<EF.Absence>
    {
        IEnumerable<Absence> ListUserAbsences(int userid);
    }
}
