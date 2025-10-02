using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Interfaces.Services
{
    public interface IBestSellingProductsService
    {
        List<BestSellingProducts> GetBestSellingProducts(int topX = 5);
    }
}
