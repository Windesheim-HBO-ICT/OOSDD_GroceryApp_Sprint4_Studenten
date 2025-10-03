
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Haalt alle gekochte producten op, eventueel gefilterd op productId
        /// </summary>
        public List<BoughtProducts> GetAll(int? productId = null)
        {
            // Haal alle grocery list items
            var items = _groceryListItemsRepository.GetAll();

            // Filter op productId indien opgegeven
            if (productId.HasValue)
            {
                items = items.Where(i => i.ProductId == productId.Value).ToList();
            }

            // Zet om naar BoughtProducts
            var boughtProducts = items.Select(i =>
            {
                var product = _productRepository.Get(i.ProductId);
                var client = _clientRepository.Get(i.Id);
                var list = _groceryListRepository.Get(i.Id);

                return new BoughtProducts(client, list, product);
                
            }).ToList();

            return boughtProducts;
        }
    }
}
