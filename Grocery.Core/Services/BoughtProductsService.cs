using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    /// <summary>
    /// Service die klanten ophaalt die een specifiek product hebben gekocht.
    /// Volgt de relatie: Product → GroceryListItem → GroceryList → Client.
    /// </summary>
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;

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

        
        /// Haalt alle klanten op die het opgegeven product hebben gekocht,
        /// inclusief de bijbehorende boodschappenlijst en productgegevens.
        /// </summary>
        /// <param name="productId">Het ID van het product.</param>
        /// <returns>Een lijst met <see cref="BoughtProducts"/> objecten.</returns>
        public List<BoughtProducts> Get(int? productId)
        {
            if (productId <= 0)
                return new List<BoughtProducts>();

            var product = _productRepository.Get(productId.Value);
            if (product == null)
                return new List<BoughtProducts>();

            var items = _groceryListItemsRepository.GetAll()
                .Where(item => item.ProductId == productId)
                .ToList();

            var result = new List<BoughtProducts>();
            foreach (var item in items)
            {
                var groceryList = _groceryListRepository.Get(item.GroceryListId);
                if (groceryList == null)
                    continue;

                var client = _clientRepository.Get(groceryList.ClientId);
                if (client != null)
                {
                    result.Add(new BoughtProducts(client, groceryList, product));
                }
            }

            return result;
        }
    }
}
