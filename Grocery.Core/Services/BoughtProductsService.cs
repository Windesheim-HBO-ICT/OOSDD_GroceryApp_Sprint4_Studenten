
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        
        
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        
        /// <summary>
        /// Creates a list of BoughtProducts from all items in the GroceryListItemsRepository
        /// </summary>
        /// <param name="productId">Products with matching productId will be returned. If null, uses all products</param>
        /// <returns>Returns list of BoughtProducts</returns>
        public List<BoughtProduct> Get(int? productId = null)
        {
            List<GroceryListItem> allItems = _groceryListItemsRepository.GetAll();
            List<BoughtProduct> boughtProducts = ItemsListToBoughtProductList(allItems);

            if (productId != null)
                boughtProducts = boughtProducts.Where(x => x.Product.Id == productId).ToList();
            
            return boughtProducts;
        }
        
        /// <summary>
        /// Creates a list of BoughtProducts from given GroceryListItems
        /// Uses GroceryListRepository, ClientRepository and ProductRepository
        /// </summary>
        /// <returns>A list of all given items in a BoughtProduct</returns>
        private List<BoughtProduct> ItemsListToBoughtProductList(List<GroceryListItem> allItems)
        {
            List<BoughtProduct> boughtProducts = new List<BoughtProduct>();
            
            foreach (var groceryListItem in allItems)
            {
                GroceryList? groceryList = GetGroceryListById(groceryListItem.GroceryListId);
                if (groceryList == null)
                    continue;
                
                Client? client = GetClientById(groceryList.ClientId);
                if (client == null)
                    continue;

                if (groceryListItem.Product.Id == 0)
                {
                    Product? product = _productRepository.Get(groceryListItem.ProductId);
                    if (product == null)
                    {
                        Console.WriteLine($"Invalid product id given to {nameof(_productRepository.Get)}");
                        continue;
                    }
                
                    groceryListItem.Product = product;
                }
                
                BoughtProduct boughtProduct = new BoughtProduct(client, groceryList, groceryListItem.Product);
                boughtProducts.Add(boughtProduct);
            }

            return boughtProducts;
        }
        
        /// <summary>
        /// Turns a BoughtProduct list into a BestSellingProduct list. 
        /// Does not use any outside respositories
        /// </summary>
        /// <returns>A list of BestSellingProducts based on given BoughtProducts</returns>
        public List<BestSellingProduct> BoughtProductsToBestSellingProducts(List<BoughtProduct> boughtProducts)
        {
            List<BestSellingProduct> bestSellingProducts = new List<BestSellingProduct>();
            
            foreach (var boughtProduct in boughtProducts)
            {
                Product product = boughtProduct.Product;
                BestSellingProduct bestSellingProduct =
                    bestSellingProducts.FirstOrDefault(x => x.Id == product.Id, 
                        new BestSellingProduct(product.Id, product.Name, product.Stock, 1, -1));
                
                if (bestSellingProducts.Contains(bestSellingProduct) == false)
                    bestSellingProducts.Add(bestSellingProduct);
                else
                    bestSellingProduct.NrOfSells++;
            }
            
            return bestSellingProducts;
        }
        
        /// <summary>
        /// Gets GroceryList from GroceryListRepository singleton with given id
        /// Writes to console in case of null being returned
        /// </summary>
        /// <returns>GroceryList with matching id</returns>
        private GroceryList? GetGroceryListById(int id)
        {
            GroceryList? groceryList = _groceryListRepository.Get(id);
            if (groceryList == null)
            {
                Console.WriteLine($"Invalid grocery list id found for GroceryListId: {id}");
                return null;
            }
            
            return groceryList;
        }
        
        /// <summary>
        /// Gets client from ClientRepository with matching id
        /// Writes to console in the case that null is returned
        /// </summary>
        /// <returns></returns>
        private Client? GetClientById(int id)
        {
            Client? clientOfProduct = _clientRepository.Get(id);
            if (clientOfProduct == null)
            {
                Console.WriteLine($"Invalid Client productowner found for GroceryListId: {id}");
                return null;
            }

            return clientOfProduct;
        }
    }
}
