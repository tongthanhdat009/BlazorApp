namespace BlazorApp.Services
{
    public interface ICartStateService
    {
        event Action? OnCartChanged;
        void NotifyCartChanged();
    }

    public class CartStateService : ICartStateService
    {
        public event Action? OnCartChanged;

        public void NotifyCartChanged()
        {
            OnCartChanged?.Invoke();
        }
    }
}
