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
    public class FileService : IFileService
    {
        IFileRepository _fileRepos;

        public FileService(IFileRepository fileRepos)
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

        public int UpdateWith(int FileId, HttpPostedFileBase IOfile, string destinationFolder, int updatedBy, int fileTypeId)
        {
            EF.File oldFile = _fileRepos.FindBy(x => x.id == FileId).FirstOrDefault();

            if (oldFile == null)
            {
                return -1;
            }

            string oldpath = Path.Combine(destinationFolder, FileId + oldFile.extension);
            string backuppath = Path.Combine(destinationFolder, FileId + "_bak" + oldFile.extension);

            oldFile.extension = System.IO.Path.GetExtension(IOfile.FileName);
            oldFile.shortname = IOfile.FileName;
            oldFile.size = IOfile.ContentLength;
            oldFile.typeid = fileTypeId;
            oldFile.lastupdated = DateTime.Now;
            oldFile.lastupdatedby = updatedBy;
            _fileRepos.Save();

            //rename old file
            FileInfo info = new FileInfo(oldpath);
            info.MoveTo(backuppath);

            IOfile.SaveAs(Path.Combine(destinationFolder, FileId + Path.GetExtension(IOfile.FileName)));

            return FileId;
        }

        #region Download

        public byte[] ReadFileBytes(string filePath){
            return System.IO.File.ReadAllBytes(filePath);
        }

        #endregion 
    }
}
