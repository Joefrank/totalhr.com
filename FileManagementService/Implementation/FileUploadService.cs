using FileManagementService.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using totalhr.data.Repositories.Infrastructure;
using EF = totalhr.data.EF;

namespace FileManagementService.Implementation
{
    public class FileUploadService : IFileUploadService
    {
        IFileRepository _fileRepos;

        public FileUploadService(IFileRepository fileRepos)
        {
            _fileRepos = fileRepos;
        }

        public int Create(EF.File file)
        {
            return _fileRepos.CreateFile(file);
        }

        public int Create(HttpPostedFileBase IOfile, string destinationFolder, int createdBy, int fileTypeId)
        {
            EF.File file = new EF.File();
            file.created = DateTime.Now;
            file.createdby = createdBy;
            file.extension = System.IO.Path.GetExtension(IOfile.FileName);
            file.shortname = IOfile.FileName;
            file.size = IOfile.ContentLength;
            file.typeid = fileTypeId;
            int newFileId = _fileRepos.CreateFile(file);

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            IOfile.SaveAs(Path.Combine(destinationFolder, newFileId + Path.GetExtension(IOfile.FileName)));
            return newFileId;
        }
    }
}
