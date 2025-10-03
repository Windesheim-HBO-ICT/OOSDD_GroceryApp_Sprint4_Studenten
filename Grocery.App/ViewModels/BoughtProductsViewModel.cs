using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;


namespace Grocery.App.ViewModels
{
    public partial class BoughtProductsViewModel : BaseViewModel
    {
        private readonly IBoughtProductsService _boughtProductsService;

        [ObservableProperty]
        Product selectedProduct;
        public ObservableCollection<BoughtProducts> BoughtProductsList { get; set; } = [];
        public ObservableCollection<Product> Products { get; set; }

        public BoughtProductsViewModel(IBoughtProductsService boughtProductsService, IProductService productService)
        {
            _boughtProductsService = boughtProductsService;
            Products = new(productService.GetAll());
        }

        partial void OnSelectedProductChanged(Product? oldValue, Product newValue)
        {
            if (newValue == null)
            {
                BoughtProductsList.Clear();
                return;
            }

            // Haal alle gekocht-producten voor het geselecteerde product
            var boughtProducts = _boughtProductsService.GetAll()
                .Where(bp => bp.Product.Id == newValue.Id)
                .ToList();

            // Update de ObservableCollection
            BoughtProductsList.Clear();
            foreach (var bp in boughtProducts)
            {
                BoughtProductsList.Add(bp);
            }
            //Zorg dat de lijst BoughtProductsList met de gegevens die passen bij het geselecteerde product. 
        }

        [RelayCommand]
        public void NewSelectedProduct(Product product)
        {
            SelectedProduct = product;
        }
    }
}
