using Grocery.Core.Interfaces.Repositories;
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

            var result = new List<BoughtProducts>();

            // haal alle boodschappenlijsten op
            var allLists = _groceryListRepository.GetAll();

            foreach (var list in allLists)
            {
                // voor elk item in de lijst
                foreach (var item in list.Items)
                {
                    if (item.Product.Id == productId)
                    {
                        var client = list.Client; // de eigenaar van de lijst
                        var product = item.Product;

                        result.Add(new BoughtProducts(client, list, product));
                    }
                }
            }

            return result;
        }
    }
}
