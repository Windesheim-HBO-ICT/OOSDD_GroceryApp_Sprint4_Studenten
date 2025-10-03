using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            var allItems = _groceriesRepository.GetAll();

            var bestSelling = allItems
                .GroupBy(item => item.ProductId)
                .Select(group =>
                {
                    // Haal het product op uit de repository
                    var product = _productRepository.Get(group.Key);

                    // Als product niet gevonden wordt, gebruik fallback
                    string productName = product?.Name ?? "Onbekend product";
                    int stock = product?.Stock ?? 0;

                    // nrOfSells = aantal keer dat het product voorkomt in de lijst
                    int nrOfSells = group.Count();

                    // Ranking wordt hier nog niet bepaald, dus zetten we op 0
                    return new BestSellingProducts(
                        group.Key,          // productId
                        productName,        // naam
                        stock,              // voorraad
                        nrOfSells,          // aantal keer verkocht
                        0                   // ranking (wordt later eventueel bepaald)
                    );
                })
                .OrderByDescending(p => p.NrOfSells)
                .Take(topX)
                .ToList();

            // Ranking toekennen (1,2,3...)
            for (int i = 0; i < bestSelling.Count; i++)
            {
                bestSelling[i].Ranking = i + 1;
            }

            return bestSelling;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
