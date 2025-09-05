using Framework.Domain.Tools;
using System.Collections.Generic;
using System.Net;

namespace Framework.Domain.Dtos.Common
{
    public class ResultModel<T>
    {
        public T? Data { get; set; }
        public int Status { get; set; } = (int)HttpStatusCode.OK;
        public string? Title { get { return Facilities.GetErrorTitle(Status); } }
        public string? Message { get; set; }
        public Dictionary<string, KeyString>? ValidationErrors { get; set; } = [];

        public ResultModel()
        {
        }

        public ResultModel(T data)
        {
            Data = data;
        }

        public ResultModel(HttpStatusCode status, string? message)
        {
            Status = (int)status;
            Message = message;
        }

        public ResultModel(HttpStatusCode status, Dictionary<string, KeyString> validationErrors)
        {
            Status = (int)status;
            ValidationErrors = validationErrors;
        }
    }

    public class KeyString
    {
        public List<string> Errors { get; set; } = [];
    }

}
