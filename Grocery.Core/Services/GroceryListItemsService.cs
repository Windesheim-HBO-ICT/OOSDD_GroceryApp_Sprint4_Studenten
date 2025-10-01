using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    /// <summary>
    /// Service die verantwoordelijk is voor het beheren van boodschappenlijstitems,
    /// inclusief het ophalen van de meest verkochte producten.
    /// </summary>
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initialiseert een nieuwe instantie van <see cref="GroceryListItemsService"/>.
        /// </summary>
        /// <param name="groceriesRepository">De repository voor toegang tot boodschappenlijstitems.</param>
        /// <param name="productRepository">De repository voor toegang tot productgegevens.</param>
        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Haalt alle boodschappenlijstitems op en vult de bijbehorende productgegevens in.
        /// </summary>
        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        /// <summary>
        /// Haalt alle boodschappenlijstitems op voor een specifieke boodschappenlijst.
        /// </summary>
        /// <param name="groceryListId">Het ID van de boodschappenlijst.</param>
        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll()
                .Where(g => g.GroceryListId == groceryListId)
                .ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        /// <summary>
        /// Voegt een nieuw boodschappenlijstitem toe aan de repository.
        /// </summary>
        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        /// <summary>
        /// Verwijdert een boodschappenlijstitem. Momenteel niet geïmplementeerd.
        /// </summary>
        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Haalt een specifiek boodschappenlijstitem op op basis van ID.
        /// </summary>
        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        /// <summary>
        /// Werkt een bestaand boodschappenlijstitem bij.
        /// </summary>
        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        /// <summary>
        /// Bepaalt de meest verkochte producten op basis van de totale hoeveelheid 
        /// vermeld in boodschappenlijstitems. Alleen items met een positieve hoeveelheid 
        /// en geldig ProductId worden meegenomen.
        /// </summary>
        /// <param name="topX">Het aantal producten dat geretourneerd moet worden (standaard 5).</param>
        /// <returns>Een lijst van <see cref="BestSellingProducts"/> gesorteerd op verkoopvolume.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Wordt gegenereerd als topX kleiner of gelijk is aan nul.
        /// </exception>
        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            if (topX <= 0)
                throw new ArgumentOutOfRangeException(nameof(topX), "Aantal moet groter zijn dan 0.");

            // Haal alle boodschappenlijstitems op uit de repository
            var allItems = _groceriesRepository.GetAll();

            // Filter en groepeer items om totale verkoop per product te berekenen
            var salesSummary = allItems
                .Where(item => item.Amount > 0 && item.ProductId > 0) // Alleen geldige verkopen
                .GroupBy(item => item.ProductId)                      // Groepeer op product
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalSold = group.Sum(item => item.Amount)        // Totaal verkochte hoeveelheid
                })
                .OrderByDescending(x => x.TotalSold)                  // Sorteer op meest verkocht
                .Take(topX)                                           // Beperk tot top X
                .ToList();

            // Zet de samenvatting om naar het BestSellingProducts-model
            var result = new List<BestSellingProducts>();
            for (int i = 0; i < salesSummary.Count; i++)
            {
                var sale = salesSummary[i];
                var product = _productRepository.Get(sale.ProductId);

                // Voeg alleen toe als het product bestaat in de productrepository
                if (product != null)
                {
                    result.Add(new BestSellingProducts(
                        productId: product.Id,
                        name: product.Name,
                        stock: product.Stock,
                        nrOfSells: sale.TotalSold,
                        ranking: i + 1 // Rangnummer start bij 1
                    ));
                }
            }
            return result;
        }

        /// <summary>
        /// Vult de Product-eigenschap van elk GroceryListItem met de bijbehorende 
        /// productgegevens uit de productrepository. Dit zorgt voor navigatie in de UI.
        /// </summary>
        /// <param name="groceryListItems">De lijst met items waarvoor productgegevens moeten worden ingevuld.</param>
        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem item in groceryListItems)
            {
                // Haal product op; gebruik standaardwaarde als niet gevonden
                item.Product = _productRepository.Get(item.ProductId) ?? new Product(0, "Onbekend", 0);
            }
        }
    }
}
