using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace totalhr.Shared.Models
{
    public class DocumentSearchInfo
    {
        public string Name { get; set; }

        public int FolderId { get; set; }

        public int AuthorId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true,  DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }
                
        public string FileMimeType { get; set; }

        public List<System.Web.Mvc.SelectListItem> Folders { get; set; }

        public List<System.Web.Mvc.SelectListItem> Contributors { get; set; }

    }
}
