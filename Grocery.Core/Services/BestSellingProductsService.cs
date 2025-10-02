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
    public class BestSellingProductsService: IBestSellingProductsService
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
            // Er moet een loop gemaakt worden die GroceryListItems ophaalt en voor elk product kijkt hoe vaak het voorkomt
            // Alles ophalen kan met GetAll
            
            List<GroceryListItem> groceryListItems = _groceryListItemsRepository.GetAll();

            // loopen door groceryListItems en een dictionary bijhouden van productId en count
            Dictionary<int, int> productCount = new Dictionary<int, int>();

            foreach (GroceryListItem item in groceryListItems)
            {
                if (!productCount.ContainsKey(item.ProductId))
                {
                    productCount[item.ProductId] = 0;
                }
                productCount[item.ProductId] += item.Amount;
            }

            // Maak een lijst met BestSellingProducts aan
            List<BestSellingProducts> bestSellingProducts = new List<BestSellingProducts>();

            // dictionery sorteren op value (aantal keer verkocht)
            var productCountList = productCount.OrderByDescending(x => x.Value).ToList();



            for (int i = 0; i < productCountList.Count(); i++)
            {
                    int productId = productCountList[i].Key;
                int amountOfSells = productCountList[i].Value;

                Product product = _productRepository.Get(productId);
                bestSellingProducts.Add(new BestSellingProducts(product.Id, product.Name, product.Stock, amountOfSells, i + 1));
            }

            return bestSellingProducts.Take(topX).ToList();
        }
    }
}