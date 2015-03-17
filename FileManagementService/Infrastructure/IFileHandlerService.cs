using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FileManagementService.Infrastructure
{
    public interface IFileHandlerService
    {
        int HandleFileCreation(HttpPostedFileBase postedFile, int creatorId, int fileTypeId);
    }
}
