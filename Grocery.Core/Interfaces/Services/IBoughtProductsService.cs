using Grocery.Core.Models;
using System.Collections.Generic;

namespace Grocery.Core.Interfaces.Services
{
    public interface IBoughtProductsService
    {
        // Haal alle gekochte producten per productId
        List<BoughtProductItem> Get(int? productId);

        // Haal alle beschikbare producten (voor Picker)
        List<Product> GetAllProducts();
    }
}
