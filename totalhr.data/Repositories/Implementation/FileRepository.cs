using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.Repositories.Infrastructure;

namespace totalhr.data.Repositories.Implementation
{
    public class FileRepository : GenericRepository<EF.TotalHREntities, EF.File>, IFileRepository
    {
        public int CreateFile(EF.File file)
        {
            this.Add(file);
            this.Save();
            return file.id;
        }
    }
}
