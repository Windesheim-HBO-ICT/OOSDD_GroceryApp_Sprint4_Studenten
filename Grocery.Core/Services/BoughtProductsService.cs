using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Grocery.Core.Services
{
    public class BoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;

        public BoughtProductsService(
            IGroceryListItemsRepository groceryListItemsRepository,
            IGroceryListRepository groceryListRepository,
            IClientRepository clientRepository,
            IProductRepository productRepository)
        {
            _groceryListItemsRepository = groceryListItemsRepository;
            _groceryListRepository = groceryListRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
        }

        public List<BoughtProductItem> Get(int? productId)
        {
            if (productId == null) return new List<BoughtProductItem>();

            var items = _groceryListItemsRepository.GetAll()
                .Where(i => i.ProductId == productId.Value)
                .ToList();

            var result = new List<BoughtProductItem>();

            foreach (var item in items)
            {
                var groceryList = _groceryListRepository.Get(item.GroceryListId);
                if (groceryList == null) continue;

                var client = _clientRepository.Get(groceryList.ClientId);
                var product = _productRepository.Get(item.ProductId);

                if (client != null && groceryList != null && product != null)
                {
                    result.Add(new BoughtProductItem
                    {
                        Client = client,
                        GroceryList = groceryList,
                        Product = product
                    });
                }
            }

            return result;
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }
    }
}
