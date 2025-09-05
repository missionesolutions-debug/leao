using Framework.Domain.Tools;
using System.Collections.Generic;
using System.Net;

namespace Framework.Domain.Dtos.Common
{
    public class ErrorResult
    {
        public int Status { get; set; } = (int)HttpStatusCode.OK;
        public string? Title { get { return Facilities.GetErrorTitle(Status); } }
        public string? Message { get; set; }
        public Dictionary<string, ErrorKeyString>? ValidationErrors { get; set; } = [];

        public ErrorResult()
        {
        }

        public ErrorResult(HttpStatusCode status, string? message)
        {
            Status = (int)status;
            Message = message;
        }

        public ErrorResult(HttpStatusCode status, Dictionary<string, ErrorKeyString> validationErrors)
        {
            Status = (int)status;
            ValidationErrors = validationErrors;
        }
    }

    public class ErrorKeyString
    {
        public List<string> Errors { get; set; } = [];
    }

}
