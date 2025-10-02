using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();

        // BindingContext instellen
        BindingContext = new LoginViewModel();
    }
}
