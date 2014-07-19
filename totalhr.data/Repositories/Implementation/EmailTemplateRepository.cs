using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.EF;

namespace totalhr.data.Repositories.Implementation
{
    public class EmailTemplateRepository : GenericRepository<TotalHREntities, EmailTemplate>, IEmailRepository
    {
    }
}
