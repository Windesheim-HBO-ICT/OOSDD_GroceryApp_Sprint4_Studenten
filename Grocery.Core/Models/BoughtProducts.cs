namespace Grocery.Core.Models
{
    public class BoughtProducts
    {
        public Client Client { get; set; }
        public GroceryList GroceryList { get; set; }
        public Product Product { get; set; }

        public BoughtProducts() { }

        public BoughtProducts(Client client, GroceryList groceryList, Product product)
        {
            Client = client;
            GroceryList = groceryList;
            Product = product;
        }
    }
}
