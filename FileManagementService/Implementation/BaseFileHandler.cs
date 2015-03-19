using FileManagementService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using totalhr.Shared;

namespace FileManagementService.Implementation
{
    public class BaseFileHandler : IFileHandlerService
    {
        protected IFileService FileService;
        protected string UploadPath = ConfigurationManager.AppSettings["DefaultUploadPath"];

        public string DirectoryPath
        {
            get { return UploadPath; }
        }
        
        public BaseFileHandler(IFileService fileService)
        {
            FileService = fileService;
        }

        public void OverridePath(string newPath)
        {
            this.UploadPath = newPath;
        }

        public int FileTypeId { get; set; }

        public virtual FileSaveResult HandleFileCreation(HttpPostedFileBase postedFile, int creatorId)
        {
            if (postedFile != null && postedFile.ContentLength > 0)
            {
                var pathString = HttpContext.Current.Server.MapPath(DirectoryPath);
                FileTypeId = (FileTypeId > 0) ? FileTypeId : (int) Variables.FileType.CompanyDocument;

                var fileId = this.FileService.Create(postedFile, pathString, creatorId, FileTypeId);

                if (fileId > 0)
                {
                    var picturePath = Path.Combine(pathString, fileId + Path.GetExtension(postedFile.FileName));
                    return new FileSaveResult { FileId = fileId, FullPath = picturePath };                    
                }
            }
            return new FileSaveResult { FileId = -1 };
        }

        public class FileSaveResult
        {
            public int FileId { get; set; }

            public string FullPath { get; set; }
        }
    }
}
