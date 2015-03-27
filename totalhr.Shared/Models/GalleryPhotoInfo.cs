using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class GalleryPhotoInfo
    {
        public int PhotoId { get; set; }

        public int AlbumId { get; set; }

        public int FileId { get; set; }

        public string FileName { get; set; }

        public int UserId { get; set; }
    }
}
