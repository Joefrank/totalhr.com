using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Resources;

namespace totalhr.Shared.Models
{
    public class DocumentShareInfo
    {
        [Required(ErrorMessageResourceType = typeof(Document), ErrorMessageResourceName = "Error_DocId_Rq")]
        public int DocumentId { get; set; }

        public string DocumentName { get; set; }

        public int WithUserId { get; set; }

        public string WithUserEmail { get; set; }

        public string Message { get; set; }

        [Required(ErrorMessageResourceType = typeof(Document), ErrorMessageResourceName = "Error_ShareType_Rq")]
        public int ShareType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Document), ErrorMessageResourceName = "Error_FileName_Rq")]
        public string FileName { get; set; }

        public string DocumentLink { get; set; }
    }
}
