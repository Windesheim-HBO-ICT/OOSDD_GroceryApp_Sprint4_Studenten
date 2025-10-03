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

        public List<BoughtProducts> Get(int? productId)
        {
            if (productId == null) return new List<BoughtProducts>();

            var result = new List<BoughtProducts>();

            // Alle clients ophalen
            var allClients = _clientRepository.GetAll();

            foreach (var client in allClients)
            {
                // Voor elke boodschappenlijst van de client
                var lists = _groceryListRepository.GetByClientId(client.Id);
                if (lists == null) continue;

                foreach (var list in lists)
                {
                    var items = _groceryListItemsRepository.GetByListId(list.Id);
                    if (items == null) continue;

                    foreach (var item in items)
                    {
                        if (item.Product != null && item.Product.Id == productId)
                        {
                            result.Add(new BoughtProducts(client, list, item.Product));
                        }
                    }
                }
            }

            return result;
        }
    }
}
