using System.Windows;

namespace EducationalPlatform.Services
{
    internal class MessageBoxService : IMessageBoxService
    {
        public void ShowError(string messageBoxText)
        {
            MessageBox.Show(messageBoxText, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error); ;
        }

        public void ShowInformation(string messageBoxText)
        {
            MessageBox.Show(messageBoxText, "Information", MessageBoxButton.OKCancel, MessageBoxImage.Information);
        }

        public void ShowWarning(string messageBoxText)
        {
            MessageBox.Show(messageBoxText, "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        }
    }
}

