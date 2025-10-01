using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class BoughtProductsView : ContentPage
{
    public BoughtProductsView()
    {
        InitializeComponent();
        // opahlen bia ViewModel  Dependency Injection
        BindingContext = App.Current.Handler.MauiContext.Services.GetRequiredService<BoughtProductsViewModel>();
    }
}