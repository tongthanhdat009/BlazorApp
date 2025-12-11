using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp;
using BlazorApp.Services;
using Blazored.LocalStorage;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient with base address to API
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("http://localhost:7000/") // Thay đổi port theo API của bạn
});

// Register services
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
await builder.Build().RunAsync();