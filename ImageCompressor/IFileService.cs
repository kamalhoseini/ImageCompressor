
namespace ImageCompressor;

public interface IFileService
{
    Task<byte[]?> CompressImageFromUrl(string imageUrl, int width, int height, int? quality, CancellationToken cancellationToken);
}