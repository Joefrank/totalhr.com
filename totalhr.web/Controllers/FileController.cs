using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace totalhr.web.Controllers
{
    public class FileController : Controller
    {
        //
        // GET: /File/

        public ActionResult Index()
        {
            using (CuteWebUI.MvcUploader uploader = new CuteWebUI.MvcUploader(System.Web.HttpContext.Current))
            {
                uploader.UploadUrl = Response.ApplyAppPathModifier("~/UploadHandler.ashx");
                uploader.Name = "myuploader";
                uploader.AllowedFileExtensions = "*.jpg,*.gif,*.png,*.bmp,*.zip,*.rar";
                uploader.InsertText = "Select a file to upload";
                ViewData["uploaderhtml"] = uploader.Render();
            } 
            return View();
        }

        public ActionResult UploadFile()
        {
            using (CuteWebUI.MvcUploader uploader = new CuteWebUI.MvcUploader(System.Web.HttpContext.Current))
            {
                uploader.UploadUrl = Response.ApplyAppPathModifier("~/Handlers/UploadHandler.ashx");
                uploader.Name = "myuploader";
                uploader.AllowedFileExtensions = "*.jpg,*.gif,*.png,*.bmp,*.zip,*.rar";
                uploader.InsertText = "Select a file to upload";
                ViewData["uploaderhtml"] = uploader.Render();
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult UploadFile(string myuploader)
        {
            using (CuteWebUI.MvcUploader uploader = new CuteWebUI.MvcUploader(System.Web.HttpContext.Current))
            {
                uploader.UploadUrl = Response.ApplyAppPathModifier("~/Handlers/UploadHandler.ashx");
                //the data of the uploader will render as <input type='hidden' name='myuploader'> 
                uploader.Name = "myuploader";
                uploader.AllowedFileExtensions = "*.jpg,*.gif,*.png,*.bmp,*.zip,*.rar";
                //allow select multiple files
                uploader.MultipleFilesUpload = true;
                //tell uploader attach a button
                uploader.InsertButtonID = "uploadbutton";
                //prepair html code for the view
                ViewData["uploaderhtml"] = uploader.Render();
                //if it's HTTP POST:
                if (!string.IsNullOrEmpty(myuploader))
                {
                    List<string> processedfiles = new List<string>();
                    //for multiple files , the value is string : guid/guid/guid 
                    foreach (string strguid in myuploader.Split('/'))
                    {
                        Guid fileguid = new Guid(strguid);
                        CuteWebUI.MvcUploadFile file = uploader.GetUploadedFile(fileguid);
                        if (file != null)
                        {
                            //you should validate it here:
                            //now the file is in temporary directory, you need move it to target location
                            //file.MoveTo("~/myfolder/" + file.FileName);
                            processedfiles.Add(file.FileName);
                        }
                    }
                    if (processedfiles.Count > 0)
                    {
                        ViewData["UploadedMessage"] = string.Join(",", processedfiles.ToArray()) + " have been processed.";
                    }
                }
            }
            return View("Index");
        }
    }
}
