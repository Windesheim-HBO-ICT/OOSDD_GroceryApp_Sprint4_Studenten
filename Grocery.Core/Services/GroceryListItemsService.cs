using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBoughtProductsService _boughtProductsService;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository, IBoughtProductsService boughtProductsService)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
            _boughtProductsService =  boughtProductsService;
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

        public List<BestSellingProduct> GetBestSellingProducts(int topX = 5)
        {
            List<BoughtProduct> boughtProducts = _boughtProductsService.Get();
            List<BestSellingProduct> bestSellingProducts = _boughtProductsService.BoughtProductsToBestSellingProducts(boughtProducts);
            
            UpdateRankingOfBestSellingProducts(bestSellingProducts);
            
            return bestSellingProducts;
        }
        
        private void UpdateRankingOfBestSellingProducts(List<BestSellingProduct> bestSellingProducts)
        {
            bestSellingProducts = bestSellingProducts.OrderByDescending(x => x.NrOfSells).ToList();

            for (int i = 1; i <= bestSellingProducts.Count; i++)
            {
                bestSellingProducts[i - 1].Ranking = i;
            }
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
