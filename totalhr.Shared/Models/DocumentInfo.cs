using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using totalhr.Resources;

namespace totalhr.Shared.Models
{
    public class DocumentInfo
    {
        public int DocId { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Document), ErrorMessageResourceName = "Error_DisplayName_Rq")]
        public string DisplayName { get; set; }
        
        public string OriginalFileName { get; set; }
        
        public int CreatedBy { get; set; }

        public int FolderId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Document), ErrorMessageResourceName = "Error_FileIsRequried_Rq")]
        public HttpPostedFileBase File { get; set; }

        public int NewFileId { get; set; }

        public int PermissionSelection { get; set; }

        public string PermissionSelectionValue { get; set; }       
        
    }

    public class DocumentInfoUpdate : DocumentInfo
    {
        public int LastUpdatedBy { get; set; }

        public int OldFileId { get; set; }

        public string ExistingFileName { get; set; }

        new public HttpPostedFileBase File { get; set; }

        public List<string> PermissionItemNames { get; set; }
       
    }
}
