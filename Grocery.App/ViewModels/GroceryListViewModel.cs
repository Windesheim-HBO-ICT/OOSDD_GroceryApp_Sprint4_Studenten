using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListViewModel : BaseViewModel
    {
        public ObservableCollection<GroceryList> GroceryLists { get; set; }
        private readonly IGroceryListService _groceryListService;

        [ObservableProperty]
        Client client;
        private readonly IClientService _clientService;

        public GroceryListViewModel(IGroceryListService groceryListService, IClientService clientService) 
        {
            Title = "Boodschappenlijst";
            _groceryListService = groceryListService;
            _clientService = clientService;
            GroceryLists = new(_groceryListService.GetAll());

            client = _clientService.Get(GroceryLists.First().ClientId);
        }

        [RelayCommand]
        public async Task SelectGroceryList(GroceryList groceryList)
        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), groceryList } };
            await Shell.Current.GoToAsync($"{nameof(Views.GroceryListItemsView)}?Titel={groceryList.Name}", true, paramater);
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

        [RelayCommand]
        public void ShowBoughtProducts()
        {
            Console.WriteLine($"ShowBoughtProducts");
            if (Client != null)
            {
                Console.WriteLine($"user -> {Client.Name}");
                if (Client.Role == ClientRole.Admin)
                {
                    Shell.Current.GoToAsync($"{nameof(Views.BoughtProductsView)}", true);
                }
            }
        }
    }
}
