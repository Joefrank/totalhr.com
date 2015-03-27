using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace ImageGallery.Infrastructure
{
    public interface IGalleryService 
    {
        int CreateAlbum(GalleryAlbumInfo info);

        int UpdateAlbum(GalleryAlbumInfo info);

        IEnumerable<GalleryAlbum> GetAlbums();

        int AddPhoto(GalleryPhotoInfo info);

        GalleryPhoto GetPhoto(int photoId);
        
        IEnumerable<GalleryPhoto> GetPhotos(int albumId);

        GalleryAlbum GetAlbum(int id);

        bool DeletePhoto(int id);
    }
}
