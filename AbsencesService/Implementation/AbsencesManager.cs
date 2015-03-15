using AbsencesService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.Shared.Models;

namespace Absences.Implementation
{
    public class AbsencesManager : IAbsencesManager
    {

        IAbsencesRepository _absenceRepos;

        public AbsencesManager(IAbsencesRepository absenceRepos)
        {
            _absenceRepos = absenceRepos;
        }
        
        public int CreateAbsence(AbsenceInfo info)
        {
            var absence = new Absence
            {
                StartDate = info.StartDate,
                EndDate = info.EndDate,
                TypeId = info.TypeId,
                IncludeSaturday = info.IncludeSaturday,
                IncludeSunday = info.IncludeSunday,
                Reason = info.Reason,
                Created = DateTime.Now,
                CreatedBy = info.CreatorId,
                UserId = info.CreatorId,
                StatusId = info.StatusId
            };

            _absenceRepos.Add(absence);

            _absenceRepos.Save();

            return absence.Id;
        }

        public IEnumerable<Absence> ListUserAbsences(int userId)
        {
            return _absenceRepos.ListUserAbsences(userId);
        }

        public void ProcessAbsenceRequest()
        {

        }
    }
}
