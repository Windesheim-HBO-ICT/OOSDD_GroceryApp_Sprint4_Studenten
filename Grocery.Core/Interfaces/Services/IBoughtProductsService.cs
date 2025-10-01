
using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IBoughtProductsService
    {
        List<BoughtProducts> Get(int productId);
        List<BoughtProducts> Get(int? productId);
    }
}
