using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Grocery.Core.Services;

namespace Grocery.App.ViewModels
{
    public partial class BoughtProductsViewModel : ObservableObject
    {
        private readonly BoughtProductsService _service;

        [ObservableProperty]
        private ObservableCollection<BoughtProductItem> products = new();

        public BoughtProductsViewModel(BoughtProductsService service)
        {
            _service = service;
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
    }
}
