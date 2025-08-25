using Newtonsoft.Json;
using System.Threading.Tasks;
using RestSharp;
using Panel.Services.Streaming.PandaVideo.Domain;

namespace Panel.Services.Streaming.PandaVideo
{
    public class PandaVideoService
    {
        private string _token;

        public PandaVideoService()
        {
        }

        public void SetToken(string token)
        {
            _token = token;
        }

        public async Task<PandaFolderResponse> GetFoldersAsync()
        {
            var options = new RestClientOptions("https://api-v2.pandavideo.com.br/folders");
            var client = new RestClient(options);
            var request = new RestRequest();
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"{_token}");

            var response = await client.GetAsync(request);

            if (response.IsSuccessful && response.Content != null)
            {
                return JsonConvert.DeserializeObject<PandaFolderResponse>(response.Content);
            }
            else
            {
                throw new Exception($"Failed to retrieve folders. Status Code: {response.StatusCode}, Error: {response.ErrorMessage}");
            }
        }

        public async Task<PandaVideosResponse> GetVideosByFolderIdAsync(string folderId)
        {
            var options = new RestClientOptions("https://api-v2.pandavideo.com.br/videos");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"{_token}");
            request.AddParameter("folder_id", folderId);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful && response.Content != null)
            {
                return JsonConvert.DeserializeObject<PandaVideosResponse>(response.Content);
            }
            else
            {
                throw new Exception($"Failed to retrieve videos for folder {folderId}. Status Code: {response.StatusCode}, Error: {response.ErrorMessage}");
            }
        }


    }
}
