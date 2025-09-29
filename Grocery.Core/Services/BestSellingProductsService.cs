using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Services
{
    public class BestSellingProductsService : IBestSellingProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IProductRepository _productRepository;

        public BestSellingProductsService(IGroceryListItemsRepository groceryListItemsRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository = groceryListItemsRepository;
            _productRepository = productRepository;
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            // Can't return 0 or less products in list
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(topX);

            List<Product> products = _productRepository.GetAll();
            Dictionary<int, int> salesPerProduct = new Dictionary<int, int>();
            List<BestSellingProducts> bestSellingProducts = new List<BestSellingProducts>();

            // Initialize sales for each products as 0
            foreach (Product product in products)
                salesPerProduct.Add(product.Id, 0);

            // Get amount of sales for each product and add them to salesPerProduct dictionary
            foreach (GroceryListItem item in _groceryListItemsRepository.GetAll())
                salesPerProduct[item.ProductId] += item.Amount;

            // Sort products based on amount of sales (in descending order, so index 0 will have highest sales)
            products.Sort((p1, p2) => salesPerProduct[p2.Id].CompareTo(salesPerProduct[p1.Id]));

            // Add best selling products to list
            for (int i = 0; i < Math.Min(topX, products.Count()); i++)
            {
                bestSellingProducts.Add(
                    new BestSellingProducts(products[i].Id, products[i].Name, products[i].Stock, salesPerProduct[products[i].Id], i + 1)
                );
            }

            return bestSellingProducts;
        }
    }
}
