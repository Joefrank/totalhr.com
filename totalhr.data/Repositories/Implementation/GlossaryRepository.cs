using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.Shared;

namespace totalhr.data.Repositories.Implementation
{
    public class GlossaryRepository : GenericRepository<TotalHREntities, Glossary>, IGlossaryRepository       
    {
        public Glossary GetByLanguage(int languageid, int glossaryRootId)
        {
            return FindBy(x => x.LanguageId == languageid && x.RootId == glossaryRootId).FirstOrDefault();
        }

        public List<Glossary> GetAllByLanguageAndGroup(int languageid, Variables.GlossaryGroups group)
        {
           return FindBy(x => x.LanguageId == languageid && group.ToString().ToLower().Equals(x.GlossaryGroup.ToLower())).ToList();
        }
    }
}
