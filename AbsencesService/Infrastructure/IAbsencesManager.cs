using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace AbsencesService.Infrastructure
{
    public interface IAbsencesManager
    {
        int CreateAbsence(AbsenceInfo info);

        IEnumerable<Absence> ListUserAbsences(int userId);

        void ProcessAbsenceRequest();
    }
}
