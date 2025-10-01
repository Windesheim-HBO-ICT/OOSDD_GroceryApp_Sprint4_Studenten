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
        private Product selectedProduct;

        public ObservableCollection<BoughtProducts> BoughtProductsList { get; set; } = new();

        public ObservableCollection<Product> Products { get; set; } = new();

        public BoughtProductsViewModel(IBoughtProductsService boughtProductsService)
        {
            _boughtProductsService = boughtProductsService;
        }

        partial void OnSelectedProductChanged(Product product)
        {
            if (product == null)
                return;

            var items = _boughtProductsService.Get(product.Id);
            BoughtProductsList.Clear();

            foreach (var item in items)
            {
                BoughtProductsList.Add(item);
            }
        }

        [RelayCommand]
        public void NewSelectedProduct(Product product)
        {
            SelectedProduct = product;
        }
    }
}
