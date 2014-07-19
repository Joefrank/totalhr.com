using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;

namespace totalhr.data.Repositories.Implementation
{
    public class LanguageRepository : GenericRepository<TotalHREntities, Language>, ILanguageRepository 
    {
        public Language GetByLanguageByGlossaryRootId(int glossaryRootId)
        {
            return FindBy(x => x.RelatedGlossaryId == glossaryRootId).FirstOrDefault();
        }

        public List<Language> GetAllByViewingLanguage(int viewingLanguageid)
        {
            return null; // FindBy(x => x. == viewingLanguageid);
        }
    }
}
