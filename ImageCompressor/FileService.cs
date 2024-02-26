using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace ImageCompressor;

public class FileService : IFileService
{
    public async Task<byte[]?> CompressImageFromUrl(string imageUrl, int width, int height, int? quality, CancellationToken cancellationToken)
    {
        var imgStream = await DownloadImageFromUrl(imageUrl, cancellationToken);
        if (imgStream is null)
            return default;

        var arr = await CompressImage(imgStream, width, height, quality, cancellationToken);

        return arr;
    }

    public async Task<MemoryStream?> DownloadImageFromUrl(string url, CancellationToken cancellationToken)
    {
        url = url.StartsWith("http") ? url : $"https:{url}";
        using (HttpClient client = new HttpClient())
        {
            using (Stream stream = await client.GetStreamAsync(url, cancellationToken))
            {
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }
    }

    public async Task<byte[]> CompressImage(MemoryStream imageStream, int width, int height, int? quality, CancellationToken cancellationToken)
    {
        using (Image image = Image.Load(imageStream))
        {
            image.Mutate(x => x.Resize(width, height));

            var encoder = new JpegEncoder { Quality = quality ?? 75 };
            using (var outputMemoryStream = new MemoryStream())
            {
                await image.SaveAsJpegAsync(outputMemoryStream, encoder, cancellationToken);
                return outputMemoryStream.ToArray();
            }
        }

    }
}



