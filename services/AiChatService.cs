using BlazorApp.Dto;

namespace BlazorApp.Services
{
    /// <summary>
    /// Interface cho AI Chat Service
    /// </summary>
    public interface IAiChatService
    {
        /// <summary>
        /// Gửi tin nhắn đến AI và nhận response
        /// </summary>
        Task<AiChatResponseDto?> SendMessageAsync(AiChatRequestDto request);
        
        /// <summary>
        /// Thêm sản phẩm được gợi ý vào giỏ hàng
        /// </summary>
        Task<AddToCartResponseDto?> AddSuggestedProductsToCartAsync(List<AddProductItem> products);
    }

    /// <summary>
    /// Service gọi AI Chat API
    /// </summary>
    public class AiChatService : IAiChatService
    {
        private readonly IApiService _apiService;

        public AiChatService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<AiChatResponseDto?> SendMessageAsync(AiChatRequestDto request)
        {
            try
            {
                return await _apiService.PostAsync<AiChatRequestDto, AiChatResponseDto>(
                    "api/customer/ai/chat", 
                    request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI Chat error: {ex.Message}");
                return new AiChatResponseDto
                {
                    Message = "Xin lỗi, không thể kết nối đến AI. Vui lòng thử lại sau.",
                    HasProductSuggestion = false
                };
            }
        }

        public async Task<AddToCartResponseDto?> AddSuggestedProductsToCartAsync(List<AddProductItem> products)
        {
            try
            {
                var request = new AddSuggestedProductsRequest { Products = products };
                return await _apiService.PostAsync<AddSuggestedProductsRequest, AddToCartResponseDto>(
                    "api/customer/ai/add-to-cart",
                    request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add to cart error: {ex.Message}");
                return null;
            }
        }
    }
}
