using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Painel.Controllers
{
    public class VideoController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public VideoController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        // Don't use `File` here. You're hiding the base `File` method.
        public async Task<ActionResult> FileUpload(IFormFile file)
        {
            List<string> errors = new List<string>(); // added this just to return something

            string fileName = null;

            if (file != null)
            {
                // Get original file name to get the extension from it.
                string orgFileName = file.FileName;

                // Create a new file name to avoid existing files on the server with the same names.
                fileName = DateTime.Now.ToFileTime() + Path.GetExtension(orgFileName);

                string fullPath = GetFullPathOfFile(fileName);

                // Create the directory.
                Directory.CreateDirectory(Directory.GetParent(fullPath).FullName);

                // Save the file to the server.
                await using FileStream output = System.IO.File.Create(fullPath);
                await file.CopyToAsync(output);
            }

            var response = new { FileName = fileName };

            return Ok(response);

            //return Json(errors, JsonRequestBehavior.AllowGet);
        }

        private string GetFullPathOfFile(string fileName)
        {
            return $"{_hostEnvironment.WebRootPath}\\uploads\\videos\\{fileName}";
        }

        [HttpPost]
        public ActionResult CKImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;
            //if (!upload.IsImage())
            //{
            //    var NotImageMessage = "please choose a picture";
            //    dynamic NotImage = JsonConvert.DeserializeObject("{ 'uploaded': 0, 'error': { 'message': \"" + NotImageMessage + "\"}}");
            //    return Json(NotImage);
            //}

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            //Image image = Image.FromStream(upload.OpenReadStream());
            //int width = image.Width;
            //int height = image.Height;
            //if ((width > 750) || (height > 500))
            //{
            //    var DimensionErrorMessage = "Custom Message for error"
            //    dynamic stuff = JsonConvert.DeserializeObject("{ 'uploaded': 0, 'error': { 'message': \"" + DimensionErrorMessage + "\"}}");
            //    return Json(stuff);
            //}

            //if (upload.Length > 500 * 1024)
            //{
            //    var LengthErrorMessage = "Custom Message for error";
            //    dynamic stuff = JsonConvert.DeserializeObject("{ 'uploaded': 0, 'error': { 'message': \"" + LengthErrorMessage + "\"}}");
            //    return Json(stuff);
            //}


            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/images/editor",
                fileName);

            Directory.CreateDirectory(Directory.GetParent(path).FullName);


            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }

            var url = $"{"/images/editor/"}{fileName}";
            var successMessage = "image is uploaded successfully";
            dynamic success = JsonConvert.DeserializeObject("{ 'uploaded': 1,'fileName': \"" + fileName + "\",'url': \"" + url + "\", 'error': { 'message': \"" + successMessage + "\"}}");
            //return Json(success);

            return Json(JsonConvert.SerializeObject("{ 'uploaded': 1,'fileName': \"" + fileName + "\",'url': \"" + url + "\", 'error': { 'message': \"" + successMessage + "\"}}"));
        }
    }
}
