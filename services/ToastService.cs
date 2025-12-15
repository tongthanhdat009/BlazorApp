namespace BlazorApp.Services
{
    public interface IToastService
    {
        event Action<string, string, ToastType>? OnShow;
        void ShowSuccess(string message, string title = "Thành công");
        void ShowError(string message, string title = "Lỗi");
        void ShowWarning(string message, string title = "Cảnh báo");
        void ShowInfo(string message, string title = "Thông báo");
    }

    public class ToastService : IToastService
    {
        public event Action<string, string, ToastType>? OnShow;

        public void ShowSuccess(string message, string title = "Thành công")
        {
            OnShow?.Invoke(message, title, ToastType.Success);
        }

        public void ShowError(string message, string title = "Lỗi")
        {
            OnShow?.Invoke(message, title, ToastType.Error);
        }

        public void ShowWarning(string message, string title = "Cảnh báo")
        {
            OnShow?.Invoke(message, title, ToastType.Warning);
        }

        public void ShowInfo(string message, string title = "Thông báo")
        {
            OnShow?.Invoke(message, title, ToastType.Info);
        }
    }

    public enum ToastType
    {
        Success,
        Error,
        Warning,
        Info
    }
}
