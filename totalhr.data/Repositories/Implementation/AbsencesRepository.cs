using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;

namespace totalhr.data.Repositories.Implementation
{
    public class AbsencesRepository :  GenericRepository<TotalHREntities, Absence>, IAbsencesRepository
    {
        public IEnumerable<Absence> ListUserAbsences(int userid)
        {
            return this.FindBy(x => x.UserId == userid);
        }
    }
}
