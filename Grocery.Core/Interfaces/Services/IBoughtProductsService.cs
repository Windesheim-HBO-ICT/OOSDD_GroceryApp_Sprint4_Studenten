
using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IBoughtProductsService
    {
        public List<BoughtProduct> Get(int? productId = null);
        
        public List<BestSellingProduct> BoughtProductsToBestSellingProducts(List<BoughtProduct> boughtProducts);
    }
}
