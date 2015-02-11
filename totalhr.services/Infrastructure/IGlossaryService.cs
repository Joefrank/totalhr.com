using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared;
using totalhr.Shared.Infrastructure;
using totalhr.Shared.Models;

namespace totalhr.services.Infrastructure
{
    public interface IGlossaryService
    {
        List<Glossary> GetGlossary(int languageid, Variables.GlossaryGroups group);

        IEnumerable<ListItemStruct> GetLanguageList(int viewingLanguageId);

        void SetCacheHelper(ICacheHelper helper);

        Glossary GetSpecificGlossary(int languageId, int glossaryRootId, Variables.GlossaryGroups group);

        string GetSpecificGlossaryTerm(int languageId, int glossaryRootId, Variables.GlossaryGroups group);

        IEnumerable<Language> GetAllLanguages();
    }
}
