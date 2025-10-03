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

        public List<BoughtProducts> Get(int? productId)
        {
            if (productId == null)
                return new List<BoughtProducts>();

            // Alle boodschappenlijst-items met dit productId ophalen
            var items = _groceryListItemsRepository.GetAll()
                .Where(i => i.ProductId == productId)
                .ToList();

            List<BoughtProducts> result = new();

            foreach (var item in items)
            {
                // Haal de GroceryList erbij
                var groceryList = _groceryListRepository.Get(item.GroceryListId);
                if (groceryList == null) continue;

                // Haal de Client erbij
                var client = _clientRepository.Get(groceryList.ClientId);
                if (client == null) continue;

                // Haal het Product erbij
                var product = _productRepository.Get(item.ProductId);
                if (product == null) continue;

                // Voeg samen in BoughtProducts
                result.Add(new BoughtProducts(client, groceryList, product));
            }

            return result;
        }
    }
}

