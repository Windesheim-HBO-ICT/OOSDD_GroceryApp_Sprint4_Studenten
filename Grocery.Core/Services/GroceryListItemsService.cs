using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Markup;

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
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            List<BestSellingProducts> bestSellingProductsList = new List<BestSellingProducts>();

            var combinedItems = groceryListItems
                .GroupBy(item => item.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalAmount = group.Sum(item => item.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            int i = 1;
            foreach(var item in combinedItems)
            {
                if(i == topX+1)
                {
                    break;
                }
                bestSellingProductsList.Add(new BestSellingProducts(item.ProductId, _productRepository.Get(item.ProductId).name, _productRepository.Get(item.ProductId).Stock, item.TotalAmount, i));
                i++;
            }

            bestSellingProductsList = bestSellingProductsList.OrderBy(p => p.Ranking).ToList();
            return bestSellingProductsList;
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
