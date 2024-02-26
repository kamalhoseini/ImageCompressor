using Microsoft.AspNetCore.Mvc;

namespace ImageCompressor.Controllers;
[ApiController]
[Route("[controller]")]
public class ImageCompressorController : ControllerBase
{
    private readonly IFileService _fileService;
    public ImageCompressorController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet(Name = "GetCompressImage")]
    public async Task<FileContentResult> Get([FromQuery] string imageUrl, [FromQuery] int width, [FromQuery] int height, [FromQuery] int? quality)
    {
        var streamData = await _fileService.CompressImageFromUrl(imageUrl, width, height, quality, default);

        if (streamData is null)
            return default!;

        FileContentResult file = new FileContentResult(streamData, "application/octet-stream")
        {
            FileDownloadName = Path.GetFileName(imageUrl) + ".jpg"
        };

        return file;
    }

}
