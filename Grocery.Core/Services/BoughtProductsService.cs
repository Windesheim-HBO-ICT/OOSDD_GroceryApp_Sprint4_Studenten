using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
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

        public IEnumerable<BoughtProducts> Get(int productId)
        {
            var groceryLists = _groceryListRepository.GetAll();

            return groceryLists
                .Where(list => list.Products.Any(p => p.Id == productId))
                .Select(list => new BoughtProducts(
                    list.Client,
                    list,
                    list.Products.First(p => p.Id == productId)
                ));
        }



        List<BoughtProducts> IBoughtProductsService.Get(int? productId)
        {
            throw new NotImplementedException();
        }

        List<BoughtProducts> IBoughtProductsService.Get(int productId)
        {
            throw new NotImplementedException();
        }
    }
}
