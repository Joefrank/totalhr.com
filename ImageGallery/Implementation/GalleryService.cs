using ImageGallery.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.Shared.Models;

namespace ImageGallery.Implementation
{
    public class GalleryService : IGalleryService
    {
        private IGalleryRepository _galleryRepos;

        public GalleryService(IGalleryRepository galleryRepos)
        {
            _galleryRepos = galleryRepos;
        }

        public int CreateAlbum(GalleryAlbumInfo info)
        {
            var album = new GalleryAlbum
            {
                Name = info.Name,
                Description = info.Description,
                Created = DateTime.Now,
                CreatedBy = info.UserId, 
                NoPhotos =0
            };

            _galleryRepos.Add(album);
            _galleryRepos.Save();

            return album.Id;
        }

        public int UpdateAlbum(GalleryAlbumInfo info)
        {
            var album = _galleryRepos.FindBy(x => x.Id == info.AlbumId).FirstOrDefault();

            if (album != null)
            {
                album.LastUpdated = DateTime.Now;
                album.LastUpdatedBy = info.UserId;
                album.Name = info.Name;
                album.Description = info.Description;
                _galleryRepos.Save();

                return 1;
            }

            return 0;
        }

        public GalleryAlbum GetAlbum(int id)
        {
            return _galleryRepos.FindBy(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<GalleryAlbum> GetAlbums()
        {
            return _galleryRepos.GetAll();
        }

        public int AddPhoto(GalleryPhotoInfo info)
        {
            var photo = new GalleryPhoto
            {
                 AlbumId = info.AlbumId,
                 Created = DateTime.Now,
                 CreatedBy = info.UserId,
                 FileId = info.FileId                 
            };

           return _galleryRepos.AddPhoto(photo);
        }

        public GalleryPhoto GetPhoto(int photoId)
        {
            return _galleryRepos.GetPhoto(photoId);
        }

        public IEnumerable<GalleryPhoto> GetPhotos(int albumId)
        {
            return _galleryRepos.GetPhotos(albumId);
        }

        
    }
}
