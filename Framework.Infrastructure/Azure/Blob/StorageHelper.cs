using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Framework.Infrastructure
{
    public static class StorageHelper
    {
      
            // Verifica se o formato corresponde ao formato suportado
            public static bool SuportedFormat(IFormFile file)
            {
                if (file.ContentType.Contains("image") || file.ContentType.Contains("application/pdf") || file.ContentType.Contains("video") || file.ContentType.Contains("audio") || file.ContentType.Contains("application/octet-stream"))
                {
                    return true;
                }
                return false;
            }

            public static async Task<Uri> UploadFileToStorage(Stream fileStream, string fileName,
                                                                AzureStorageConfig _storageConfig, string contentType = null)
            {
                try
                {
                    // Cria o link do arquivo.
                    Uri blobUri = new Uri("https://" +
                                          _storageConfig.AccountName +
                                          ".blob.core.windows.net/" +
                                          _storageConfig.ContainerName +
                                          "/" + fileName);


                    // Constroi os Headers com as credenciais para acesso a conta de armazenamento
                    StorageSharedKeyCredential storageCredentials =
                        new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);

                    // Cria o blob client necessario para fazer o upload
                    // como a uri do arquivo e as credenciais.
                    BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

                    //Adiciona o formato do arquivo a requisicao.
                    BlobHttpHeaders blobHttpHeader = new() { ContentType = contentType };

                    // Faz o Upload do arquivo.
                    await blobClient.UploadAsync(fileStream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });

                    return blobUri;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            // Metodo necessario pois sem informar o formato ele nao
            // faz o upload corretamente.
            private static BlobHttpHeaders GetFormats(string file)
            {
                var headers = new BlobHttpHeaders();
                var extension = Path.GetExtension(file);
                headers.ContentType = GetContentType(extension);


                return headers;
            }

            private static string GetContentType(string extension) => extension switch
            {
                //imagem
                ".jpg" => "image/jpg",
                ".png" => "image/png",
                ".jpeg" => "image/jpeg",
                ".webp" => "image/webp",
                //video
                ".mp4" => "video/mp4",
                ".webm" => "video/webm",
                ".ogg" => "video/ogg",
                //audio
                ".weba" => "audio/webm;codecs=opus",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".mp3" => "audio/mpeg",
                ".aac" => "audio/aac",
                ".flac" => "audio/flac",
                ".wav" => "audio/wav",
                ".aiff" => "audio/aiff",
                ".amr" => "audio/amr",
                _ => throw new Exception("Tipo não encontrado")
            };
        }
    }
