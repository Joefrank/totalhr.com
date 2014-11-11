using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using totalhr.data;
using totalhr.data.EF;

namespace FileManagementService.Infrastructure
{
    public interface IFileUploadService
    {
        int Create(File file);

        int Create(HttpPostedFileBase IOfile, string destinationFolder, int createdBy, int fileTypeId);
    }
}
