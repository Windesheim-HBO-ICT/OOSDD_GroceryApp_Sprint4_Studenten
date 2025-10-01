using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Models
{
    public class BoughtProductItem
    {
        public Client Client { get; set; }
        public GroceryList GroceryList { get; set; }
        public Product Product { get; set; } 
    }
}
