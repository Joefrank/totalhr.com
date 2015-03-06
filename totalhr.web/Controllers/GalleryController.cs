using System.IO;
using Authentication.Infrastructure;
using ImageGallery.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.Shared.Models;

namespace totalhr.web.Controllers
{
    public class GalleryController : BaseController
    {
        private IGalleryService _galleryService;

        public GalleryController(IGalleryService galleryService, IOAuthService authservice)
            : base(authservice)
        {
            _galleryService = galleryService;
        }

        public ActionResult Index()
        {
            return View(_galleryService.GetAlbums());
        }

        public ActionResult ViewAlbumPhotos(int id)
        {
            return View("ViewAlbum", _galleryService.GetAlbum(id));
        }

        public ActionResult CreateAlbum()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAlbum(GalleryAlbumInfo info)
        {
            info.UserId = CurrentUser.UserId;
            info.NoOfPhotos = 0;

            var result = _galleryService.CreateAlbum(info);

            return RedirectToAction("Index");
        }

        public ActionResult UploadPhoto()
        {
            return View();
        }

      
        public ActionResult EditAlbum(int id)
        {
            return View(_galleryService.GetAlbum(id));
        }

        public ActionResult CreateAlbumPhoto(int id)
        {
            return null;
            //return View("CreateAlbumPhoto", _galleryService.GetAlbum(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">This is photo id</param>
        /// <returns></returns>
        //public JsonResult RemoveAlbumPhoto(int id)
        //{
        //    return new Newtonsoft.Json { };
        //}
    }
}
