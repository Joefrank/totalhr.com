using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface IGlossaryRepository : IGenericRepository<Glossary>
    {
        Glossary GetByLanguage(int languageid, int glossaryRootId);

        List<Glossary> GetAllByLanguageAndGroup(int languageid, Variables.GlossaryGroups group);
    }
}
