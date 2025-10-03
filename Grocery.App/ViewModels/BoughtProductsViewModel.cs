using CommunityToolkit.Mvvm.ComponentModel;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    /// <summary>
    /// ViewModel voor de weergave van klanten die een specifiek product hebben gekocht.
    /// Laadt productlijst bij opstart en vult klantgegevens bij productselectie.
    /// </summary>
    public partial class BoughtProductsViewModel : BaseViewModel
    {
        private readonly IBoughtProductsService _boughtProductsService;

        /// <summary>
        /// Het momenteel geselecteerde product.
        /// Wijziging activeert automatisch het laden van bijbehorende klanten.
        /// </summary>
        [ObservableProperty]
        private Product? selectedProduct;

        /// <summary>
        /// Lijst met klanten en boodschappenlijsten die het geselecteerde product hebben gekocht.
        /// </summary>
        public ObservableCollection<BoughtProducts> BoughtProductsList { get; } = new();

        /// <summary>
        /// Beschikbare producten voor selectie in de Picker.
        /// </summary>
        public ObservableCollection<Product> Products { get; }

        public BoughtProductsViewModel(
            IBoughtProductsService boughtProductsService,
            IProductService productService)
        {
            _boughtProductsService = boughtProductsService;
            Products = new(productService.GetAll()); // Direct initialiseren

            System.Diagnostics.Debug.WriteLine($"Aantal producten: {Products.Count}"); //tijdelijke test
            foreach (var p in Products)
            {
                System.Diagnostics.Debug.WriteLine($"Product: {p.Name}"); //tijdelijke test
            }
        }

        /// <summary>
        /// Wordt automatisch aangeroepen wanneer SelectedProduct wijzigt.
        /// Haalt klantgegevens op voor het nieuwe product.
        /// </summary>
        partial void OnSelectedProductChanged(Product? oldValue, Product? newValue)
        {
            BoughtProductsList.Clear();

            if (newValue != null)
            {
                var boughtProducts = _boughtProductsService.Get(newValue.Id);
                foreach (var item in boughtProducts)
                {
                    BoughtProductsList.Add(item);
                }
            }
        }
    }
}