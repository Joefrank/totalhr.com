using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;

namespace totalhr.data.Repositories.Implementation
{
    public class GalleryRepository : GenericRepository<TotalHREntities, GalleryAlbum>, IGalleryRepository
    {
        public GalleryPhoto GetPhoto(int id)
        {
            return this.Context.GalleryPhotoes.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<GalleryPhoto> GetPhotos(int albumId)
        {
            return this.Context.GalleryPhotoes.Where(x => x.AlbumId == albumId);
        }

        public int AddPhoto(GalleryPhoto photo)
        {
            this.Context.GalleryPhotoes.Add(photo);
            return this.Context.SaveChanges();
        }

        public bool DeletePhoto(int id)
        {
            this.Context.GalleryPhotoes.Remove(
                this.Context.GalleryPhotoes.FirstOrDefault(x => x.Id == id));
            return this.Context.SaveChanges() > 0;
        }
    }
}
