using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Framework.Infrastructure.Tools;
using Microsoft.AspNetCore.Http;

namespace Framework.Infrastructure
{
    public class BlobService
    {
        public async Task<dynamic> Upload(List<IFormFile> files)
        {
            // Pega as informações do blob.
            AzureStorageConfig storageConfig = GetBlobConfiguration();

            #region Validacao
            if (files == null || files.Count == 0)
                throw new Exception("Nenhum arquivo enviado");

            if (storageConfig.AccountKey == string.Empty || storageConfig.AccountName == string.Empty)
                throw new Exception("Erro interno 1");

            if (storageConfig.ContainerName == string.Empty)
                throw new Exception("Erro Interno 2");
            #endregion

            List<Uri> fileLinks = new List<Uri>();

            foreach (var file in files)
            {
                // Verifica se o formato é suportado.
                if (StorageHelper.SuportedFormat(file))
                {
                    // Verifica se o tamanho está abaixo do permitido.
                    //if (file.Length < 1000000)
                    //{
                        using (Stream stream = file.OpenReadStream())
                        {
                            // Cria um novo nome para o arquivo.
                            string fileName = String.Concat(Path.GetRandomFileName().LimparUrl().RemoverAcentos(), Path.GetExtension(file.FileName));

                            // Faz o upload do arquivo e recupera o link do blob.
                            Uri filelink = await StorageHelper.UploadFileToStorage(stream, fileName, storageConfig, file.ContentType);
                            fileLinks.Add(filelink);
                        }
                    //}
                    //else
                    //{
                    //    throw new Exception("Arquivo maior que 10mb");
                    //}
                }
                else
                {
                    throw new Exception("Formato não suportado");
                }
            }

            string links = string.Join(",", fileLinks);

            var arquivos = new
            {
                links
            };

            return arquivos;

        }

        public async Task<dynamic> Upload(MemoryStream file, string Name)
        {
            // Pega as informações do blob.
            AzureStorageConfig storageConfig = GetBlobConfiguration();

            #region Validacao
            if (file == null)
                throw new Exception("Nenhum arquivo enviado");

            if (storageConfig.AccountKey == string.Empty || storageConfig.AccountName == string.Empty)
                throw new Exception("Erro interno 1");

            if (storageConfig.ContainerName == string.Empty)
                throw new Exception("Erro Interno 2");
            #endregion

            dynamic filelink;

            using (Stream stream = new MemoryStream(file.ToArray()))
            {
                // Cria um novo nome para o arquivo.
                string fileName = String.Concat(Path.GetRandomFileName().LimparUrl().RemoverAcentos(), Path.GetExtension(Name));

                // Faz o upload do arquivo e recupera o link do blob.
                filelink = await StorageHelper.UploadFileToStorage(stream, fileName, storageConfig);
            }

            return filelink;

        }

        // Pega as configuracoes do blob
        private AzureStorageConfig GetBlobConfiguration()
        {
            AzureStorageConfig storageConfig = new();

            storageConfig.AccountName = "codie";
            storageConfig.AccountKey = "zCMttLkxwdcQXcwojkh+24tA7XOvtK0pAYkRLqniddXRQ52H1se/UD7H/RTLwDNu7uwdBNwOv2SU+ASth7EtDA==";
            storageConfig.ContainerName = "codie";

            return storageConfig;
        }

        public async Task<Uri> UploadBlobAsync(IFormFile file)
        {
            try
            {
                AzureStorageConfig storageConfig = GetBlobConfiguration();

                if (file == null)
                    throw new Exception("Nenhum arquivo enviado");

                ValidBlobCredentials();

                if (StorageHelper.SuportedFormat(file))
                {
                    using (Stream stream = file.OpenReadStream())
                    {
                        string fileName = string.Concat(Path.GetRandomFileName(), Path.GetExtension(file.FileName));

                        if (file.ContentType == "image/svg+xml")
                        {

                            Uri filelink = await StorageHelper.UploadFileToStorage(stream, fileName, storageConfig, "image/svg+xml");

                            // Substitui a URL no link retornado
                            string modifiedUrl = filelink.ToString().Replace("https://codie.blob.core.windows.net/", "https://cdn.codiehost.com.br/");

                            return new Uri(modifiedUrl);
                        }

                        if (file.ContentType == "application/pdf")
                        {
                            Uri filelink = await StorageHelper.UploadFileToStorage(stream, fileName, storageConfig, "application/pdf");
                            // Substitui a URL no link retornado
                            string modifiedUrl = filelink.ToString().Replace("https://codie.blob.core.windows.net/", "https://cdn.codiehost.com.br/");

                            return new Uri(modifiedUrl);
                        }
                        else
                        {
                            Uri filelink = await StorageHelper.UploadFileToStorage(stream, fileName, storageConfig);
                            // Substitui a URL no link retornado
                            string modifiedUrl = filelink.ToString().Replace("https://codie.blob.core.windows.net/", "https://cdn.codiehost.com.br/");

                            return new Uri(modifiedUrl);
                        }
                      
                    }
                }
                else
                {
                    throw new Exception("Formato não suportado");
                }
            }
            catch (Exception)
            {
                throw new Exception("Problema ao enviar a imagem");
            }
        }

        public async Task<bool> DeleteBlobFromStorage(string blobName)
        {
            AzureStorageConfig storageConfig = GetBlobConfiguration();

            var blobServiceClient = new BlobServiceClient(new Uri($"https://{storageConfig.AccountName}.blob.core.windows.net"), new Azure.Storage.StorageSharedKeyCredential(storageConfig.AccountName, storageConfig.AccountKey));
            var containerClient = blobServiceClient.GetBlobContainerClient(storageConfig.ContainerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            return await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }
        private void ValidBlobCredentials()
        {
            AzureStorageConfig storageConfig = GetBlobConfiguration();

            if (string.IsNullOrWhiteSpace(storageConfig.AccountKey) || string.IsNullOrWhiteSpace(storageConfig.AccountName))
                throw new Exception("Configurações da azure inválidas");

            if (string.IsNullOrWhiteSpace(storageConfig.ContainerName))
                throw new Exception("Nome do container inválido");
        }
    }
}
