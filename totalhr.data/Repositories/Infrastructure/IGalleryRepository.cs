using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface IGalleryRepository : IGenericRepository<GalleryAlbum>
    {
        GalleryPhoto GetPhoto(int id);

        IEnumerable<GalleryPhoto> GetPhotos(int albumId);

        int AddPhoto(GalleryPhoto photo);
    }
}
