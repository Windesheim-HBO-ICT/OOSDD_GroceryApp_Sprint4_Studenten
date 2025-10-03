
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
            var items = _groceryListItemsRepository.GetAll();

            if (productId.HasValue)
                items = items.Where(i => i.ProductId == productId.Value).ToList();

            return items
                .GroupBy(i => i.ProductId)
                .Select(g => new BoughtProducts(
                    client: null,
                    groceryList: null,
                    product: _productRepository.Get(g.Key) ?? new Product(0, "", 0)
                ))
                .ToList();
        }
    }
}
