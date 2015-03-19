using FileManagementService.Infrastructure;
using Microsoft.Win32;
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
        readonly IFileRepository _fileRepos;

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
            var file = new EF.File
                {
                    created = DateTime.Now,
                    createdby = createdBy,
                    extension = System.IO.Path.GetExtension(IOfile.FileName),
                    shortname = IOfile.FileName,
                    size = IOfile.ContentLength,
                    typeid = fileTypeId
                };
            var newFileId = _fileRepos.CreateFile(file);

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            IOfile.SaveAs(Path.Combine(destinationFolder, newFileId + Path.GetExtension(IOfile.FileName)));
            return newFileId;
        }

        public int UpdateWith(int FileId, HttpPostedFileBase IOfile, string destinationFolder, int updatedBy, int fileTypeId)
        {
            var oldFile = _fileRepos.FindBy(x => x.id == FileId).FirstOrDefault();

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
            var info = new FileInfo(oldpath);
            if (!File.Exists(backuppath))
            {
                info.MoveTo(backuppath);
            }

            IOfile.SaveAs(Path.Combine(destinationFolder, FileId + Path.GetExtension(IOfile.FileName)));

            return FileId;
        }

        public String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        public string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName);
 
            if (String.IsNullOrWhiteSpace(extension))
            {
                return null;
            }
 
            var registryKey = Registry.ClassesRoot.OpenSubKey(extension);
 
            if (registryKey == null)
            {
                return null;
            }
 
            var value = registryKey.GetValue("Content Type") as string;
             
            return String.IsNullOrWhiteSpace(value) ? null : value;
        }

        #region Download

        public byte[] ReadFileBytes(string filePath){
            return System.IO.File.ReadAllBytes(filePath);
        }

        #endregion 
    }
}
