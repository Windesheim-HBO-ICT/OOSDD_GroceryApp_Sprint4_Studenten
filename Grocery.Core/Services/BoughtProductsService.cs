
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        public List<BoughtProducts> Get(int? productId)
        {
            if (productId is null)
                return new List<BoughtProducts>();

            var product = _productRepository.Get(productId.Value);
            if (product is null)
                return new List<BoughtProducts>();

            return _groceryListItemsRepository
                .GetAll()
                .Where(i => i.ProductId == productId.Value)
                .Select(i =>
                {
                    var list = _groceryListRepository.Get(i.GroceryListId);
                    if (list is null) return null;

                    var client = _clientRepository.Get(list.ClientId);
                    if (client is null) return null;

                    return new BoughtProducts(client, list, product);
                })
                .Where(bp => bp is not null)
                .ToList()!;
        }

    }
}
