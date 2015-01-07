using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.services.Infrastructure;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.EF;
using totalhr.Shared;
using totalhr.Shared.Infrastructure;
using totalhr.Shared.Models;

namespace totalhr.services.Implementation
{
    public class GlossaryService : IGlossaryService
    {
        IGlossaryRepository _glossaryRepos;
        ICacheHelper _cacheHelper;

        public void SetCacheHelper(ICacheHelper helper) {
            _cacheHelper = helper; 
        }

        //use a cache handler to store glossaries in memory.

        public GlossaryService(IGlossaryRepository glossRepos, ICacheHelper cacheHelper)
        {
            _glossaryRepos = glossRepos;
            _cacheHelper = cacheHelper;
        }

        public IEnumerable<ListItemStruct> GetLanguageList(int viewingLanguageId)
        {
            return _glossaryRepos.GetLanguageList(viewingLanguageId);
        }

        public List<Glossary> GetGlossary(int languageid, Variables.GlossaryGroups group)
        {
            List<Glossary> allGlossaries;

            if (_cacheHelper.Exists("AllGlossaries"))
            {
                _cacheHelper.Get<List<Glossary>>("AllGlossaries", out allGlossaries);
            }
            else
            {
                allGlossaries = _glossaryRepos.GetAll().ToList();
                _cacheHelper.Add<List<Glossary>>(allGlossaries, "AllGlossaries");
            }
            

            return (allGlossaries != null)? allGlossaries.Where(x => x.LanguageId == languageid && 
                group.ToString().ToLower().Equals(x.GlossaryGroup.ToLower())).OrderBy(x => x.GroupOrder).ToList()
                : new List<Glossary>();
        }

        public Glossary GetSpecificGlossary(int languageId, int glossaryRootId, Variables.GlossaryGroups group)
        {
            var lstGlossary = GetGlossary(languageId, group);
            return (lstGlossary != null)? lstGlossary.FirstOrDefault(x => x.RootId == glossaryRootId) : null;            
        }

        public string GetSpecificGlossaryTerm(int languageId, int glossaryRootId, Variables.GlossaryGroups group)
        {
            if (languageId < 1 || glossaryRootId < 1)
                return "";

            var lstGlossary = GetGlossary(languageId, group);
            return (lstGlossary != null) ? lstGlossary.FirstOrDefault(x => x.RootId == glossaryRootId).Term : "";
        }
    }
}
