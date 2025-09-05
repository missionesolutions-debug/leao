namespace xCodie.Application.DTOs.Commom;

public class DownloadFileInfo(string filePath, string contentType, string fileName)
{
    public string FilePath { get; set; } = filePath;
    public string ContentType { get; set; } = contentType;
    public string FileName { get; set; } = fileName;
}
