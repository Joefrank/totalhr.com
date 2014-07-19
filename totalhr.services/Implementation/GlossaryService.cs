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
    }
}
