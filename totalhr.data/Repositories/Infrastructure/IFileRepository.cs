using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface IFileRepository : IGenericRepository<EF.File>
    {
        int CreateFile(EF.File file);
    }
}
