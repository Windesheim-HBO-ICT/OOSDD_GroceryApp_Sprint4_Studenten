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
            // Alle GroceryListItems ophalen
            var allItems = _groceriesRepository.GetAll();

            // Groeperen op ProductId en het aantal verkopen tellen
            var grouped = allItems
                .GroupBy(i => i.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    NrOfSells = g.Count()
                })
                .OrderByDescending(x => x.NrOfSells)
                .Take(topX)
                .ToList();

            // Maak BestSellingProducts objecten aan en geef ranking mee
            var bestSelling = grouped
                .Select((g, index) =>
                {
                    var product = _productRepository.Get(g.ProductId) ?? new Product(0, "", 0);
                    return new BestSellingProducts(
                        product.Id,
                        product.Name,
                        product.Stock,
                        g.NrOfSells,
                        index + 1 // Ranking begint bij 1
                    );
                })
                .ToList();

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
