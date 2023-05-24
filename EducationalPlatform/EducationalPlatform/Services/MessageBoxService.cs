using System.Windows;

namespace EducationalPlatform.Services
{
    internal class MessageBoxService : IMessageBoxService
    {
        public void ShowError(string messageBoxText)
        {
            MessageBox.Show(messageBoxText, "Eroare", MessageBoxButton.OKCancel, MessageBoxImage.Error); ;
        }

        public void ShowInformation(string messageBoxText)
        {
            MessageBox.Show(messageBoxText, "Informare", MessageBoxButton.OKCancel, MessageBoxImage.Information);
        }

        public void ShowWarning(string messageBoxText)
        {
            MessageBox.Show(messageBoxText, "Avertisment", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        }

        public bool AskConfirmation(string messageBoxText) 
        {
            MessageBoxResult result = MessageBox.Show(messageBoxText, "Confirmare", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                return true;
            }

            return false;
        }
    }
}

