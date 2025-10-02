using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListViewModel : BaseViewModel
    {
        public ObservableCollection<GroceryList> GroceryLists { get; set; }
        private readonly IGroceryListService _groceryListService;
        public GlobalViewModel globalViewModel { get; }

        public GroceryListViewModel(IGroceryListService groceryListService, GlobalViewModel global) 
        {
            Title = "Boodschappenlijst";
            globalViewModel = global;
            _groceryListService = groceryListService;
            GroceryLists = new(_groceryListService.GetByClientId(globalViewModel.Client.Id));
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
            GroceryLists = new(_groceryListService.GetByClientId(globalViewModel.Client.Id));
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            GroceryLists.Clear();
        }

        [RelayCommand]
        public async Task ShowBoughtProducts()
        {
            // If the authenticated user is an Admin send them to the BoughtProductsPage
            if (globalViewModel.Client.Role == Role.Admin)
            {
                await Shell.Current.GoToAsync($"{nameof(Views.BoughtProductsView)}?Titel={globalViewModel.Client.Name}", true);
            }
        }
    }
}
