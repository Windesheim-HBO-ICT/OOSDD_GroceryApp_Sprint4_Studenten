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

        /// <summary>
        /// Haalt de best verkopende producten op op basis van alle boodschappenlijst-items.
        /// </summary>
        /// <param name="topX">
        /// Het aantal top-producten dat moet worden teruggegeven. 
        /// Standaardwaarde = 5.
        /// </param>
        /// <returns>
        /// Een lijst van <see cref="BestSellingProducts"/> met productinformatie, 
        /// het aantal verkopen en een ranking.
        /// </returns>
        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            // Alle boodschappenlijst-items ophalen
            var allItems = _groceriesRepository.GetAll();

            // Groeperen op productId en aantal verkopen tellen
            var grouped = allItems
                .GroupBy(i => i.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    NrOfSells = g.Count()
                })
                .OrderByDescending(g => g.NrOfSells)
                .Take(topX)
                .ToList();

            // Omzetten naar BestSellingProducts en ranking toekennen
            var bestSellers = grouped
                .Select((g, index) =>
                {
                    var product = _productRepository.Get(g.ProductId);

                    return new BestSellingProducts(
                        product.Id,
                        product.Name,
                        product.Stock,
                        g.NrOfSells,
                        index + 1 // ranking start bij 1
                    );
                })
                .ToList();

            return bestSellers;
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
