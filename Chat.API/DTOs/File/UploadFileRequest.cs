namespace Chat.API.DTOs.File
{
    public record UploadFileRequest
    (
        IFormFile File,
        string? Caption = null
    );
}
