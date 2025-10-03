using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BestSellingProductsService
    {
        public List<BestSellingProducts> Bereken(List<GroceryListItem> alleItems, List<Product> alleProducten, int topX = 5)
        {
            var productSales = alleItems
                .GroupBy(item => item.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalAmount = group.Sum(item => item.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .Take(topX)
                .ToList();

            List<BestSellingProducts> bestSelling = new();
            int ranking = 1;

            foreach (var sale in productSales)
            {
                Product? product = alleProducten.FirstOrDefault(p => p.Id == sale.ProductId);
                if (product != null)
                {
                    bestSelling.Add(new BestSellingProducts(
                        product.Id,
                        product.Name,
                        product.Stock,
                        sale.TotalAmount,
                        ranking
                    ));
                    ranking++;
                }
            }

            return bestSelling;
        }
    }
}
