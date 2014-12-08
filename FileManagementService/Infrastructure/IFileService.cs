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
    public interface IFileService
    {
        int Create(File file);

        int Create(HttpPostedFileBase IOfile, string destinationFolder, int createdBy, int fileTypeId);

        int UpdateWith(int FileId, HttpPostedFileBase IOfile, string destinationFolder, int createdBy, int fileTypeId);

        byte[] ReadFileBytes(string filePath);

        String BytesToString(long byteCount);

        string GetMimeType(string fileName);
    }
}
