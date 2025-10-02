using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Grocery.App.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListViewModel : BaseViewModel
    {
        public ObservableCollection<GroceryList> GroceryLists { get; set; }
        private readonly IGroceryListService _groceryListService;

        public Client CurrentClient { get; }
        private readonly BoughtProductsService _boughtProductsService;
        public ICommand ShowBoughtProductsCommand { get; }

        public GroceryListViewModel(
            IGroceryListService groceryListService,
            Client currentClient,
            BoughtProductsService boughtProductsService) : base()
        {
            Title = "Boodschappenlijst";
            _groceryListService = groceryListService;
            GroceryLists = new(_groceryListService.GetAll());

            CurrentClient = currentClient;
            _boughtProductsService = boughtProductsService;
            ShowBoughtProductsCommand = new AsyncRelayCommand(ShowBoughtProducts);
        }

        [RelayCommand]
        public async Task SelectGroceryList(GroceryList groceryList)
        {
            Dictionary<string, object> parameter = new() { { nameof(GroceryList), groceryList } };
            await Shell.Current.GoToAsync($"{nameof(Views.GroceryListItemsView)}?Titel={groceryList.Name}", true, parameter);
        }

        private async Task ShowBoughtProducts()
        {
            if (CurrentClient.Role == Role.Admin)
            {
                var vm = new BoughtProductsViewModel(_boughtProductsService);
                var view = new BoughtProductsView
                {
                    BindingContext = vm
                };

                await Shell.Current.Navigation.PushAsync(view);
            }
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            GroceryLists = new(_groceryListService.GetAll());
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            GroceryLists.Clear();
        }
    }
}
