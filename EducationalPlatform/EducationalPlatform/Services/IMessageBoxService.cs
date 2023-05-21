namespace EducationalPlatform.Services
{
    public interface IMessageBoxService
    {
        void ShowError(string messageBoxText);
        void ShowInformation(string messageBoxText);
        void ShowWarning(string messageBoxText);
    }
}