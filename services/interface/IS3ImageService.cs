namespace BlazorApp.Services.Interface
{
    public interface IS3ImageService
    {
        /// <summary>
        /// Lấy URL đầy đủ của ảnh từ S3 key hoặc URL (async cho presigned URL)
        /// </summary>
        Task<string> GetImageUrlAsync(string? imageUrlOrKey);
        
        /// <summary>
        /// Kiểm tra xem URL có phải là S3 URL không
        /// </summary>
        bool IsS3Url(string? url);
        
        /// <summary>
        /// Lấy placeholder image khi không có ảnh
        /// </summary>
        string GetPlaceholderImage();
    }
}
