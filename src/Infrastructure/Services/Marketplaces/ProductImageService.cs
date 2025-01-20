using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.Marketplaces;

public interface IProductImageService
{
    Task<IEnumerable<int>> GetProductIdsWithImages();
    string GetImageUrlByArticul(string articul);
    string GetImageNameByArticul(string articul);
}

public class ProductImageService : IProductImageService
{
    private readonly string _imagePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "solos");
    private readonly MContext _mContext;
    private readonly string _baseUrl;

    public ProductImageService(MContext mContext, IConfiguration configuration)
    {
        _mContext = mContext;
        _baseUrl = configuration.GetValue<string>("ApiSettings:ApiUrl");
    }

    public async Task<IEnumerable<int>> GetProductIdsWithImages()
    {
        var articulsByFiles = GetFileNamesWithoutExtensions();

        var articuls = await _mContext.Products
            .AsNoTracking()
            .Where(x => articulsByFiles.Contains(x.Articul))
            .Select(x => x.Id)
            .ToListAsync();

        return articuls;
    }

    public string GetImageUrlByArticul(string articul)
    {
        var imageName = GetImageNameByArticul(articul);
        return $"{_baseUrl}/solos/{imageName}";
    }

    public string GetImageNameByArticul(string articul)
    {
        var files = Directory.GetFiles(_imagePath);
        var file = files.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x) == articul);
        return Path.GetFileName(file);
    }

    private IEnumerable<string> GetFileNamesWithoutExtensions()
    {
        var files = Directory.GetFiles(_imagePath);
        var fileNames = files.Select(Path.GetFileNameWithoutExtension);
        return fileNames;
    }
}