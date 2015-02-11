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
        private readonly IGlossaryRepository _glossaryRepos;
        private readonly ILanguageRepository _languageRepository;

        ICacheHelper _cacheHelper;



        public void SetCacheHelper(ICacheHelper helper) {
            _cacheHelper = helper; 
        }

        //use a cache handler to store glossaries in memory.

        public GlossaryService(IGlossaryRepository glossRepos, ILanguageRepository languageRepository, ICacheHelper cacheHelper)
        {
            _glossaryRepos = glossRepos;
            _languageRepository = languageRepository;
            _cacheHelper = cacheHelper;
        }

        public IEnumerable<ListItemStruct> GetLanguageList(int viewingLanguageId)
        {
            var languageGlossaries = GetGlossary(viewingLanguageId, Variables.GlossaryGroups.Language);
            var allLanguages = GetAllLanguages();

            var result = allLanguages.Select(x =>
                {
                    var firstOrDefault = languageGlossaries.FirstOrDefault(y => y.RootId == x.RelatedGlossaryId && y.LanguageId == viewingLanguageId);
                    return firstOrDefault != null ? new ListItemStruct {
                                                          Id = x.Id, 
                                                          Name = firstOrDefault.Term} : null;
                });

            return result;
        }

        public IEnumerable<Language> GetAllLanguages()
        {
            IEnumerable<Language> allLanguages;

            if (_cacheHelper.Exists("AllLanguages"))
            {
                allLanguages = _cacheHelper.Get<IEnumerable<Language>>("AllLanguages");
            }
            else
            {
                allLanguages = _languageRepository.GetAll().ToList();
                _cacheHelper.Add<IEnumerable<Language>>(allLanguages, "AllLanguages");
            }

            return allLanguages;
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
            var el = lstGlossary.FirstOrDefault(x => x.RootId == glossaryRootId);

            if (el != null)
                return (lstGlossary != null) ? el.Term : "";
            else
                return "";
        }
    }
}
