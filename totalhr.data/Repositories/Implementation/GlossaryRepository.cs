using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.Shared;
using totalhr.Shared.Models;

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

        public IEnumerable<ListItemStruct> GetLanguageList(int viewingLanguageId)
        {
            return from ll in Context.Languages
                            join gloss in Context.Glossaries
                                on ll.RelatedGlossaryId equals gloss.RootId
                            where gloss.LanguageId == viewingLanguageId
                            select new ListItemStruct { Id = ll.Id, Name = gloss.Term };
        }
    }
}
