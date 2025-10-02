using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Data.Repositories;
using Grocery.Core.Models;
using Microsoft.Maui.Controls; // voor Application.Current

namespace Grocery.App.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ClientRepository _clientRepo;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        public IAsyncRelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            _clientRepo = new ClientRepository();
            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        private async Task LoginAsync()
        {
            var client = _clientRepo.Get(Email);

            if (client == null || client.Password != Password)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Ongeldige inloggegevens", "OK");
                return;
            }

            Application.Current.Properties["CurrentClientId"] = client.Id;

            Application.Current.MainPage = new AppShell();
        }
    }
}
