using System.Net;

namespace Framework.Services.Site.Helpers
{
    public class HttpErrorService
    {
        public string GetError(Exception ex)
        {
            if (ex.InnerException != null)
            {
                switch (ex.Message)
                {
                    case "404NotFound":
                        return ex.InnerException.Message;
                    case "409Conflict":
                        return ex.InnerException.Message;
                    case "403Forbidden":
                        return ex.InnerException.Message;
                    case "204NoContent":
                        return ex.InnerException.Message;
                    case "400BadRequest":
                        return ex.InnerException.Message;
                    case "500InternalServerError":
                        return ex.InnerException.Message;
                    default:
                        break;
                }
            }
            return null;
        }

        public int GetErrorCode(Exception ex)
        {
            if (ex.InnerException != null)
            {
                switch (ex.Message)
                {
                    case "404NotFound":
                        return (int)HttpStatusCode.NotFound;
                    case "409Conflict":
                        return (int)HttpStatusCode.Conflict;
                    case "403Forbidden":
                        return (int)HttpStatusCode.Forbidden;
                    case "204NoContent":
                        return (int)HttpStatusCode.NoContent;
                    case "400BadRequest":
                        return (int)HttpStatusCode.BadRequest;
                    case "500InternalServerErro":
                        return (int)HttpStatusCode.InternalServerError;
                    default:
                        return 200;
                }
            }
            else
                return (int)HttpStatusCode.InternalServerError;
        }
    }

   
      
}
