using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface ILanguageRepository : IGenericRepository<Language>
    {
        Language GetByLanguageByGlossaryRootId(int glossaryRootId);

        List<Language> GetAllByViewingLanguage(int viewingLanguageid);
 
    }
}
