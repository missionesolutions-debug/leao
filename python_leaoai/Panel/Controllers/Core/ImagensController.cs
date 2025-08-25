using Data;
using Data.Models.Core;
using Framework.Factories.Core;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Painel.Builders;
using Painel.Builders.Core;
using Painel.Models;
using System.IO;
using System.Threading.Tasks;

namespace Painel.Controllers.Core
{
    [Authorize]
    public class ImagensController : Controller
    {
        private static ImagemBuilder _builder;
        private static ImagemFactory _factory;
        private static PagesBuilder _pgBuilder;
        private static BlobService _srvBlob;

        public ImagensController(ApplicationDbContext context)
        {
            _factory = new ImagemFactory(context);
            _builder = new ImagemBuilder(context);
            _pgBuilder = new PagesBuilder(context);
            _srvBlob = new BlobService();
        }

        public IActionResult Index(string tab, int id)
        {


            if (tab is null && id == 0)
            {
                return View("~/Views/_Core/Imagens/Index.cshtml", _pgBuilder.BuildViewImagems(new Info(), _builder.GetListWithoutTableReference()));
            }

            return View("~/Views/_Core/Imagens/Index.cshtml", _pgBuilder.BuildViewImagems(new Info(), _builder.GetListByTabAndId(tab, id), tab, id));
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile[] files, string tab, int id)
        {
            try
            {
                if (files == null || files.Length == 0)
                {
                    return Redirect("/imagens?Tab=" + tab + "&Id=" + id + "&result=ImagemFailed");
                }

                foreach (var file in files)
                {
                    var url = await _srvBlob.UploadBlobAsync(file);

                    string[] fileName = file.FileName.Split(".");

                    var guid = Guid.NewGuid().ToString();

                    _factory.SaveObj(new Imagem()
                    {
                        Guid = fileName[0],
                        Ativo = true,
                        Excluido = false,
                        FileData = url.ToString(),
                        FileSize = file.Length.ToString(),
                        FileType = fileName[1],
                        TableId = id,
                        TableAction = tab,
                        Slug = fileName[0],
                        PlaceReceived = "",
                        DataCriacao = DateTime.UtcNow.ToTimeZone("E. South America Standard Time"),
                        Url = url.ToString().Replace("https://codie.blob.core.windows.net/", "https://cdn.codiehost.com.br/")
                    });

                }
            }
            catch (Exception)
            {
                return Redirect("/imagens?Tab=" + tab + "&Id=" + id + "&result=ImagemFailed");
            }

            return Redirect("/imagens?Tab=" + tab + "&Id=" + id + "&result=ImagemSuccess");
        }

        [HttpPost]
        public IActionResult Index(IFormFile[] files, string tab, int id)
        {
            string DirectoryToPostFile = "wwwroot/imagens/" + tab + "/" + id;

            try
            {
                if (files == null || files.Length == 0)
                {
                    return Content("File(s) not selected");
                }
                else
                {
                    if (Directory.Exists(DirectoryToPostFile) is false)
                        Directory.CreateDirectory(DirectoryToPostFile);

                    foreach (IFormFile file in files)
                    {
                        // Get original file name to get the extension from it.
                        string orgFileName = file.FileName;

                        String guid = Guid.NewGuid().ToString();

                        String fileTypeName = Path.GetExtension(orgFileName);
                        String fileNameFinal = guid + fileTypeName;

						var path = Path.Combine(Directory.GetCurrentDirectory(), DirectoryToPostFile, fileNameFinal);
						using (var stream = new FileStream(path, FileMode.Create))
						{
							file.CopyTo(stream);
						}


						_builder.SaveOrUpdate(new Imagem()
                        {
                            Guid = guid,
                            Ativo = true,
                            Excluido = false,
                            FileData = fileNameFinal,
                            FileSize = file.Length.ToString(),
                            FileType = fileTypeName,
                            TableId = id,
                            TableAction = tab,
                            Slug = guid,
                            PlaceReceived = DirectoryToPostFile.Replace("wwwroot/", ""),
                            DataCriacao = DateTime.UtcNow.ToTimeZone()
                        });
                    }
                }
            }
            catch (Exception)
            {
                return Redirect("/imagens?Tab=" + tab + "&Id=" + id + "&result=ImagemFailed");
            }

            return Redirect("/imagens?Tab=" + tab + "&Id=" + id + "&result=ImagemSuccess");
        }




        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Imagem obj = _factory.GetObj(Id);

            if (obj != null)
                _builder.DeleteObj(Id);

            return Redirect("/imagens?Tab=" + obj.TableAction + "&Id=" + obj.TableId + "&result=ImagemSuccess");
        }

        [HttpPost]
        public IActionResult Rename(int Id, string Titulo, string Alt, string FileTags, string DescriptionFile)
        {
            Imagem obj = _factory.GetObj(Id);

            if (obj != null)
            {
                obj.FileName = Titulo;
                obj.Alt = Alt;
                obj.FileTags = FileTags;
                obj.DescriptionFile = DescriptionFile;

                _factory.UpdateObj(obj);
            }

            return Redirect("/imagens?Tab=" + obj.TableAction + "&Id=" + obj.TableId + "&result=ImagemSuccess");
        }
    }
}
