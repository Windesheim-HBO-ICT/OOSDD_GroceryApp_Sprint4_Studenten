using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBoughtProductsService _boughtProductService;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository, IBoughtProductsService boughtProducts)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
            _boughtProductService = boughtProducts;
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
            FillService(allItems); 

            var grouped = allItems
                .GroupBy(i => i.ProductId)
                .Select(g => new
                {
                    Product = g.First().Product,
                    NrOfSells = g.Count()
                })
                .OrderByDescending(p => p.NrOfSells)
                .Take(topX)
                .ToList();

            var result = new List<BestSellingProducts>();
            for (int i = 0; i < grouped.Count; i++)
            {
                var g = grouped[i];
                result.Add(new BestSellingProducts(
                    g.Product.Id,
                    g.Product.Name,
                    g.Product.Stock,
                    g.NrOfSells,
                    i + 1
                ));
            }

            return result;
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
