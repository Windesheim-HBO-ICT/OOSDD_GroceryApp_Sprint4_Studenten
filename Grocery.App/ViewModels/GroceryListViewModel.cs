using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListViewModel : BaseViewModel
    {
        // ObservableCollection van boodschappenlijsten
        public ObservableCollection<GroceryList> GroceryLists { get; set; }

        private readonly IGroceryListService _groceryListService;
        private readonly IClientService _clientService;

        // Huidige ingelogde gebruiker (voor testdoeleinden)
        private readonly string _currentUserEmail = "user3@mail.com";

        // Constructor
        public GroceryListViewModel(IGroceryListService groceryListService, IClientService clientService)
        {
            Title = "Boodschappenlijst";
            _groceryListService = groceryListService;
            _clientService = clientService;

            GroceryLists = new ObservableCollection<GroceryList>(_groceryListService.GetAll());
        }

        // Property voor de ToolbarItem
        public string ClientName => _clientService.Get(_currentUserEmail)?.Name ?? "Guest";

        // Command voor het selecteren van een boodschappenlijst
        [RelayCommand]
        public async Task SelectGroceryList(GroceryList groceryList)
        {
            if (groceryList == null) return;

            var parameter = new Dictionary<string, object>
            {
                { nameof(GroceryList), groceryList }
            };

            await Shell.Current.GoToAsync($"{nameof(Views.GroceryListItemsView)}?Titel={groceryList.Name}", true, parameter);
        }

        // Command voor het tonen van gekochte producten (alleen admin)
        [RelayCommand]
        public async Task ShowBoughtProducts()
        {
            var client = _clientService.Get(_currentUserEmail);
            if (client != null && client.Role == Role.Admin)
            {
                await Shell.Current.GoToAsync(nameof(Views.BoughtProductsView));
            }
            // niet-admin: niets doen
        }

        // Lifecycle overrides
        public override void OnAppearing()
        {
            base.OnAppearing();
            GroceryLists = new ObservableCollection<GroceryList>(_groceryListService.GetAll());
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            GroceryLists.Clear();
        }
    }
}
