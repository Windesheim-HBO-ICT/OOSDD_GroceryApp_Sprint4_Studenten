
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class BestSellingProductsViewModel : BaseViewModel
    {
        private readonly IGroceryListItemsService _groceryListItemsService;
        public ObservableCollection<BestSellingProducts> Products { get; set; } = [];
        public string Message { get; set; } = string.Empty;
        public BestSellingProductsViewModel(IGroceryListItemsService groceryListItemsService)
        {
            _groceryListItemsService = groceryListItemsService;
            Products = [];
            Load();
        }

        public override void Load()
        {
            Products.Clear();
            var topProducts = _groceryListItemsService.GetBestSellingProducts();

            if (topProducts.Count == 0)
            {
                Message = "Er zijn nog geen artikelen verkocht.";
            }
            else
            {
                Message = string.Empty;
                foreach (var item in topProducts)
                {
                    Products.Add(item);
                }
            }

            //foreach (BestSellingProducts item in _groceryListItemsService.GetBestSellingProducts())
            //{
            //    Products.Add(item);
            //}
        }

        public override void OnAppearing()
        {
            Load();
        }

        public override void OnDisappearing()
        {
            Products.Clear();
        }
    }
}
