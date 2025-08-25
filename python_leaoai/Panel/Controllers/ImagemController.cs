using Framework.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Painel.Controllers
{
    public class ImagemController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private static BlobService _srvBlob;

		public ImagemController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _srvBlob = new BlobService();
		}

        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var url = await _srvBlob.UploadBlobAsync(file);

                    var response = new { FileName = url.ToString() };

                    return Ok(response);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
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
