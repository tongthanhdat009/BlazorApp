namespace BlazorApp.Dto
{
    /// <summary>
    /// Request gửi tin nhắn đến AI Chat
    /// </summary>
    public class AiChatRequestDto
    {
        /// <summary>
        /// Tin nhắn từ người dùng
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// Lịch sử hội thoại (tùy chọn)
        /// </summary>
        public List<ChatMessageDto>? History { get; set; }
    }

    /// <summary>
    /// Một tin nhắn trong hội thoại
    /// </summary>
    public class ChatMessageDto
    {
        /// <summary>
        /// Vai trò: "user" hoặc "assistant"
        /// </summary>
        public string Role { get; set; } = "user";
        
        /// <summary>
        /// Nội dung tin nhắn
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response từ AI Chat
    /// </summary>
    public class AiChatResponseDto
    {
        /// <summary>
        /// Tin nhắn trả lời từ AI
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// Có gợi ý thêm sản phẩm vào giỏ hàng không
        /// </summary>
        public bool HasProductSuggestion { get; set; } = false;
        
        /// <summary>
        /// Danh sách sản phẩm được gợi ý (nếu có)
        /// </summary>
        public List<ProductSuggestionDto>? SuggestedProducts { get; set; }
        
        /// <summary>
        /// Nguồn ngữ cảnh từ RAG
        /// </summary>
        public List<ContextSourceDto>? ContextSources { get; set; }
    }

    /// <summary>
    /// Sản phẩm được AI gợi ý
    /// </summary>
    public class ProductSuggestionDto
    {
        /// <summary>
        /// ID sản phẩm
        /// </summary>
        public int ProductId { get; set; }
        
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        
        /// <summary>
        /// Số lượng gợi ý
        /// </summary>
        public int SuggestedQuantity { get; set; } = 1;
        
        /// <summary>
        /// Giá sản phẩm
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// Đơn vị
        /// </summary>
        public string? Unit { get; set; }
        
        /// <summary>
        /// URL hình ảnh
        /// </summary>
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// Nguồn ngữ cảnh từ RAG (hiển thị cho user)
    /// </summary>
    public class ContextSourceDto
    {
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        
        /// <summary>
        /// Đoạn trích mô tả
        /// </summary>
        public string Excerpt { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request thêm sản phẩm gợi ý vào giỏ hàng
    /// </summary>
    public class AddSuggestedProductsRequest
    {
        /// <summary>
        /// Danh sách sản phẩm cần thêm
        /// </summary>
        public List<AddProductItem> Products { get; set; } = new();
    }

    /// <summary>
    /// Item sản phẩm cần thêm
    /// </summary>
    public class AddProductItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Response khi thêm sản phẩm
    /// </summary>
    public class AddToCartResponseDto
    {
        public string? Message { get; set; }
        public List<AddedItemDto>? AddedItems { get; set; }
        public List<string>? Errors { get; set; }
    }

    public class AddedItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public bool Success { get; set; }
    }
}
