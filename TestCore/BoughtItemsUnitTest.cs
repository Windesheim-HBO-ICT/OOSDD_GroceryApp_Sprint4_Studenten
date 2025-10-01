using System.Collections.Generic;
using System.Linq;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Moq;
using Xunit;

namespace TestCore
{
    public class BoughtProductsServiceTests
    {
        private readonly Mock<IGroceryListItemsRepository> _groceryListItemsRepoMock;
        private readonly Mock<IClientRepository> _clientRepoMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IGroceryListRepository> _groceryListRepoMock;
        private readonly BoughtProductsService _service;

        public BoughtProductsServiceTests()
        {
            _groceryListItemsRepoMock = new Mock<IGroceryListItemsRepository>();
            _clientRepoMock = new Mock<IClientRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _groceryListRepoMock = new Mock<IGroceryListRepository>();

            _service = new BoughtProductsService(
                _groceryListItemsRepoMock.Object,
                _groceryListRepoMock.Object,
                _clientRepoMock.Object,
                _productRepoMock.Object
            );
        }

        [Fact]
        public void Get_WhenProductIdIsNull_ReturnsNull()
        {
            // Act
            var result = _service.Get(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Get_WhenValidProductId_ReturnsBoughtProductsForEachClient()
        {
            // Arrange
            int productId = 1;

            var product = new Product { Id = productId, Name = "Milk" };

            var clients = new List<Client>
            {
                new Client { Id = 10, Name = "Alice" },
                new Client { Id = 20, Name = "Bob" }
            };

            var groceryLists = new List<GroceryList>
            {
                new GroceryList { Id = 100, ClientId = 10, Name = "Alice's List" },
                new GroceryList { Id = 200, ClientId = 20, Name = "Bob's List" }
            };

            _productRepoMock.Setup(r => r.Get(productId)).Returns(product);
            _clientRepoMock.Setup(r => r.GetAll()).Returns(clients);
            _groceryListRepoMock.Setup(r => r.GetAll()).Returns(groceryLists);

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Contains(result, bp => bp.Client.Name == "Alice" && bp.GroceryList.Name == "Alice's List" && bp.Product.Name == "Milk");
            Assert.Contains(result, bp => bp.Client.Name == "Bob" && bp.GroceryList.Name == "Bob's List" && bp.Product.Name == "Milk");
        }
    }
}
