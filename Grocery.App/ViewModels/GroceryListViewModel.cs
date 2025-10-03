using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListViewModel : BaseViewModel
    {
        public ObservableCollection<GroceryList> GroceryLists { get; set; }
        public ObservableCollection<BoughtProducts> BoughtProductsList { get; set; } = new ObservableCollection<BoughtProducts>();

        private readonly IGroceryListService _groceryListService;
        private readonly IBoughtProductsService _boughtProductsService;
        private readonly Client _client;

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                OnSelectedProductChanged();
            }
        }

        public GroceryListViewModel(IGroceryListService groceryListService, IBoughtProductsService boughtProductsService, Client client)
        {
            Title = "Boodschappenlijst";
            _groceryListService = groceryListService;
            _boughtProductsService = boughtProductsService;
            _client = client;
            GroceryLists = new ObservableCollection<GroceryList>(_groceryListService.GetAll());
        }

        [RelayCommand]
        public async Task SelectGroceryList(GroceryList groceryList)
        {
            var parameter = new Dictionary<string, object> { { nameof(GroceryList), groceryList } };
            await Shell.Current.GoToAsync($"{nameof(Views.GroceryListItemsView)}?Titel={groceryList.Name}", true, parameter);
        }

        private void OnSelectedProductChanged()
        {
            BoughtProductsList.Clear();
            if (_selectedProduct == null) return;

            var boughtProducts = _boughtProductsService.Get(_selectedProduct.Id);
            foreach (var bp in boughtProducts)
            {
                BoughtProductsList.Add(bp);
            }
        }

        [RelayCommand]
        public async Task ShowBoughtProducts()
        {
            if (_client.Role == Role.Admin)
            {
                await Shell.Current.GoToAsync(nameof(Views.BoughtProductsView));
            }
            // anders niets doen
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            GroceryLists = new ObservableCollection<GroceryList>(_groceryListService.GetAll());
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            GroceryLists.Clear();
            BoughtProductsList.Clear();
        }
    }
}
