using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views; // voor BoughtProductsView
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts; //voor toast

namespace Grocery.App.ViewModels
{
    /// <summary>
    /// ViewModel voor de overzichtspagina van boodschappenlijsten.
    /// Bevat functionaliteit om klantgegevens te tonen (alleen voor Admin).
    /// </summary>
    public partial class GroceryListViewModel : BaseViewModel
    {
        private readonly IGroceryListService _groceryListService;
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// De huidige ingelogde client. Wordt ingesteld op user3 (Admin) voor testdoeleinden.
        /// In productie zou dit via authenticatie worden bepaald.
        /// </summary>
        public Client? CurrentClient { get; private set; }

        public ObservableCollection<GroceryList> GroceryLists { get; set; }

        public GroceryListViewModel(IGroceryListService groceryListService, IClientRepository clientRepository)
        {
            Title = "Boodschappenlijst";
            _groceryListService = groceryListService;
            _clientRepository = clientRepository;
            GroceryLists = new(_groceryListService.GetAll());

            // UC13: Stel user3 (Admin) in als huidige client
            CurrentClient = _clientRepository.Get(3); // user3 = A.J. Kwak (Admin)
        }

        [RelayCommand]
        public async Task SelectGroceryList(GroceryList groceryList)
        {
            var parameter = new Dictionary<string, object> { { nameof(GroceryList), groceryList } };
            await Shell.Current.GoToAsync($"{nameof(Views.GroceryListItemsView)}?Titel={groceryList.Name}", true, parameter);
        }

        /// <summary>
        /// Navigeert naar BoughtProductsView alleen als de huidige client Admin is.
        /// </summary>
        [RelayCommand]
        private async Task ShowBoughtProducts()
        {
            try
            {
                if (CurrentClient?.Role == Role.Admin)
                {
                    await Shell.Current.GoToAsync(nameof(BoughtProductsView));
                }
            }
            catch (Exception ex)
            {
                // Toon de fout in de output
                System.Diagnostics.Debug.WriteLine($"Navigatie-fout: {ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                // Optioneel: toon een toast
                await Toast.Make($"Fout: {ex.Message}").Show();
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