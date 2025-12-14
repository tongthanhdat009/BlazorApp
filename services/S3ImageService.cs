using BlazorApp.Services.Interface;
using System.Net.Http.Json;

namespace BlazorApp.Services
{
    public class S3ImageService : IS3ImageService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _bucketName;
        private readonly string _region;
        // Using a data URI to avoid 404 errors
        private readonly string _placeholderImage = "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='200' height='200' viewBox='0 0 200 200'%3E%3Crect fill='%23f0f0f0' width='200' height='200'/%3E%3Ctext x='50%25' y='50%25' dominant-baseline='middle' text-anchor='middle' font-family='Arial' font-size='16' fill='%23999'%3ENo Image%3C/text%3E%3C/svg%3E";
        private readonly Dictionary<string, (string url, DateTime expiry)> _urlCache = new();

        public S3ImageService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _bucketName = _configuration["AWS:BucketName"] ?? "dotnet-app-images";
            _region = _configuration["AWS:Region"] ?? "ap-southeast-2";
        }

        public async Task<string> GetImageUrlAsync(string? imageUrlOrKey)
        {
            if (string.IsNullOrWhiteSpace(imageUrlOrKey))
            {
                return _placeholderImage;
            }

            // Nếu đã là URL đầy đủ (http/https), return luôn
            if (imageUrlOrKey.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                imageUrlOrKey.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return imageUrlOrKey;
            }

            // Check cache
            if (_urlCache.TryGetValue(imageUrlOrKey, out var cached))
            {
                // Nếu URL còn hạn (buffer 5 phút trước khi hết hạn)
                if (cached.expiry > DateTime.UtcNow.AddMinutes(5))
                {
                    return cached.url;
                }
                else
                {
                    _urlCache.Remove(imageUrlOrKey);
                }
            }

            // Gọi API backend để lấy presigned URL
            var presignedUrl = await GetPresignedUrlAsync(imageUrlOrKey);
            return presignedUrl ?? _placeholderImage;
        }

        private async Task<string?> GetPresignedUrlAsync(string s3Key)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<PresignedUrlResponse>(
                    $"api/upload/presigned-url?s3Key={Uri.EscapeDataString(s3Key)}&expirationMinutes=60"
                );

                if (response != null && !string.IsNullOrEmpty(response.Url))
                {
                    // Cache URL với thời gian hết hạn
                    _urlCache[s3Key] = (response.Url, DateTime.UtcNow.AddMinutes(response.ExpiresIn));
                    return response.Url;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting presigned URL: {ex.Message}");
            }

            return null;
        }

        public bool IsS3Url(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            return url.Contains($"{_bucketName}.s3.{_region}.amazonaws.com", StringComparison.OrdinalIgnoreCase) ||
                   url.Contains($"s3.{_region}.amazonaws.com/{_bucketName}", StringComparison.OrdinalIgnoreCase);
        }

        public string GetPlaceholderImage()
        {
            return _placeholderImage;
        }

        private class PresignedUrlResponse
        {
            public string Url { get; set; } = string.Empty;
            public int ExpiresIn { get; set; }
        }
    }
}
