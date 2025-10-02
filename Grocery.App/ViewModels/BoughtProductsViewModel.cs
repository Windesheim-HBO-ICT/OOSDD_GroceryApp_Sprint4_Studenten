using CommunityToolkit.Mvvm.ComponentModel;
using Grocery.Core.Models;
using Grocery.Core.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace Grocery.App.ViewModels
{
    public partial class BoughtProductsViewModel : ObservableObject
    {
        private readonly BoughtProductsService _service;

        [ObservableProperty]
        private ObservableCollection<BoughtProductItem> products = new();

        [ObservableProperty]
        private ObservableCollection<Product> allProducts = new();

        public BoughtProductsViewModel(BoughtProductsService service)
        {
            _service = service;

            var productsFromService = _service.GetAllProducts();
            AllProducts = new ObservableCollection<Product>(productsFromService);
        }

        public void OnSelectedProductChanged(int productId)
        {
            var items = _service.Get(productId)
                .Select(x => new BoughtProductItem
                {
                    Client = x.Client,
                    GroceryList = x.GroceryList,
                    Product = x.Product
                })
                .ToList();

            Products = new ObservableCollection<BoughtProductItem>(items);
        }

        public void NewSelectedProduct(Product product)
        {
            if (product == null) return;
            OnSelectedProductChanged(product.Id);
        }
    }
}
