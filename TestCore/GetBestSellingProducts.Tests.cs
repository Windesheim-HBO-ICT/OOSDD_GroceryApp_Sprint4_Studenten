using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Moq;
using Spectre.Console;
using TestCore.Builders;

namespace TestCore;

[TestFixture]
public class GetBestSellingProductsTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void UC11_FR1_GetBestSellingProducts_ShouldReturnTop5Products()
    {
        // Arrange
        var groceries = GroceryListItemBuilder.CreateMany(
            (1,2),
            (2,1),
            (3,3),
            (4,1),
            (5,2),
            (6,1)
        );
        

        var groceriesRepoMock = new Mock<IGroceryListItemsRepository>();
        groceriesRepoMock.Setup(r => r.GetAll()).Returns(groceries);

        var productRepoMock = new Mock<IProductRepository>();
        productRepoMock.Setup(r => r.Get(It.IsAny<int>()))
            .Returns<int>(id => new Product(id, $"product{id}", 10));

        var service = new GroceryListItemsService(groceriesRepoMock.Object, productRepoMock.Object);

        // Act
        var result = service.GetBestSellingProducts();

        // Debug output in een mooie tabel
        var table = new Table().RoundedBorder();
        table.AddColumn("Rank");
        table.AddColumn("Product");
        table.AddColumn("Stock");
        table.AddColumn("Verkocht");

        foreach (var p in result)
        {
            table.AddRow(p.Ranking.ToString(), p.Name, p.Stock.ToString(), p.NrOfSells.ToString());
        }

        AnsiConsole.Write(table);

        // Assert
        Assert.That(result, Has.Count.EqualTo(5));
        Assert.That(result[0].NrOfSells, Is.EqualTo(3));
    }
    
    [Test]
    public void UC11_FR1_ShouldReturnEmptyList_WhenNoProducts()
    {
        // Arrange
        var groceriesRepoMock = new Mock<IGroceryListItemsRepository>();
        groceriesRepoMock.Setup(r => r.GetAll()).Returns(new List<GroceryListItem>());
        var productRepoMock = new Mock<IProductRepository>();

        var service = new GroceryListItemsService(groceriesRepoMock.Object, productRepoMock.Object);

        // Act
        var result = service.GetBestSellingProducts();
        
        // Debug output in een mooie tabel
        var table = new Table().RoundedBorder();
        table.AddColumn("Rank");
        table.AddColumn("Product");
        table.AddColumn("Stock");
        table.AddColumn("Verkocht");

        foreach (var p in result)
        {
            table.AddRow(p.Ranking.ToString(), p.Name, p.Stock.ToString(), p.NrOfSells.ToString());
        }

        AnsiConsole.Write(table);

        // Assert
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public void UC11_FR1_ShouldReturnAll_WhenLessThan5Products()
    {
        // Arrange
        var groceries = GroceryListItemBuilder.CreateMany((1,1),(2,1),(3,1));
        var groceriesRepoMock = new Mock<IGroceryListItemsRepository>();
        groceriesRepoMock.Setup(r => r.GetAll()).Returns(groceries);

        var productRepoMock = new Mock<IProductRepository>();
        productRepoMock.Setup(r => r.Get(It.IsAny<int>())).Returns<int>(id => new Product(id, $"Product{id}", 5));

        var service = new GroceryListItemsService(groceriesRepoMock.Object, productRepoMock.Object);

        // Act
        var result = service.GetBestSellingProducts();
        
        // Debug output in een mooie tabel
        var table = new Table().RoundedBorder();
        table.AddColumn("Rank");
        table.AddColumn("Product");
        table.AddColumn("Stock");
        table.AddColumn("Verkocht");

        foreach (var p in result)
        {
            table.AddRow(p.Ranking.ToString(), p.Name, p.Stock.ToString(), p.NrOfSells.ToString());
        }

        AnsiConsole.Write(table);

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
    }



    [Test]
    public void UC11_FR2_GetBestSellingProducts_ShouldReturnCorrectProductInfo()
    {
        // Arrange
        var groceries = GroceryListItemBuilder.CreateMany(
            (1, 2)
        );

        var groceriesRepoMock = new Mock<IGroceryListItemsRepository>();
        groceriesRepoMock.Setup(r => r.GetAll()).Returns(groceries);

        var productRepoMock = new Mock<IProductRepository>();
        productRepoMock.Setup(r => r.Get(1)).Returns(new Product(1, "Banaan", 10));

        var service = new GroceryListItemsService(groceriesRepoMock.Object, productRepoMock.Object);

        // Act
        var result = service.GetBestSellingProducts();
        
        // Debug output in een mooie tabel
        var table = new Table().RoundedBorder();
        table.AddColumn("Rank");
        table.AddColumn("Product");
        table.AddColumn("Stock");
        table.AddColumn("Verkocht");

        foreach (var p in result)
        {
            table.AddRow(p.Ranking.ToString(), p.Name, p.Stock.ToString(), p.NrOfSells.ToString());
        }

        AnsiConsole.Write(table);

        // Assert
        var product = result.First();
        Assert.Multiple(() =>
        {
            Assert.That(product.Id, Is.EqualTo(1));
            Assert.That(product.Name, Is.EqualTo("Banaan"));
            Assert.That(product.Stock, Is.EqualTo(10));
            Assert.That(product.NrOfSells, Is.EqualTo(2));
            Assert.That(product.Ranking, Is.EqualTo(1));
        });
    }

    [Test]
    public void UC11_FR3_AddingProduct_ShouldUpdateBoodschappenlijst()
    {
        // Arrange
        var groceries = new List<GroceryListItem>();
        var groceriesRepoMock = new Mock<IGroceryListItemsRepository>();
        groceriesRepoMock.Setup(r => r.GetAll()).Returns(groceries);

        var productRepoMock = new Mock<IProductRepository>();
        productRepoMock.Setup(r => r.Get(It.IsAny<int>()))
            .Returns<int>(id => new Product(id, $"Product{id}", 5));

        var service = new GroceryListItemsService(groceriesRepoMock.Object, productRepoMock.Object);

        // Act: nieuw product toevoegen
        groceries.Add(new GroceryListItem(1, 1, 1, 100));
        groceries.Add(new GroceryListItem(2, 1, 1, 100));

        var result = service.GetBestSellingProducts();
        
        // Debug output in een mooie tabel
        var table = new Table().RoundedBorder();
        table.AddColumn("Rank");
        table.AddColumn("Product");
        table.AddColumn("Stock");
        table.AddColumn("Verkocht");

        foreach (var p in result)
        {
            table.AddRow(p.Ranking.ToString(), p.Name, p.Stock.ToString(), p.NrOfSells.ToString());
        }

        AnsiConsole.Write(table);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].NrOfSells, Is.EqualTo(2));
    }
    
    [Test]
    public void UC11_FR3_ShouldAccumulateSells_ForSameProduct()
    {
        // Arrange
        var groceries = GroceryListItemBuilder.CreateMany((1,1),(1,1),(1,1)); // 3x zelfde product
        var groceriesRepoMock = new Mock<IGroceryListItemsRepository>();
        groceriesRepoMock.Setup(r => r.GetAll()).Returns(groceries);

        var productRepoMock = new Mock<IProductRepository>();
        productRepoMock.Setup(r => r.Get(1)).Returns(new Product(1,"Appel",10));

        var service = new GroceryListItemsService(groceriesRepoMock.Object, productRepoMock.Object);

        // Act
        var result = service.GetBestSellingProducts();
        
        // Debug output in een mooie tabel
        var table = new Table().RoundedBorder();
        table.AddColumn("Rank");
        table.AddColumn("Product");
        table.AddColumn("Stock");
        table.AddColumn("Verkocht");

        foreach (var p in result)
        {
            table.AddRow(p.Ranking.ToString(), p.Name, p.Stock.ToString(), p.NrOfSells.ToString());
        }

        AnsiConsole.Write(table);

        // Assert
        Assert.That(result[0].NrOfSells, Is.EqualTo(3));
    }


    [Test]
    public void UC11_NFR2_GetBestSellingProducts_ShouldBeSortedByNrOfSellsDescending()
    {
        // Arrange
        var groceries = new List<GroceryListItem>
        {
            new(1, 1, 1, 100),
            new(2, 1, 1, 100), // 2x
        
            new(3, 2, 2, 100), // 1x
        
            new(4, 3, 3, 100),
            new(5, 3, 3, 100),
            new(6, 3, 3, 100)  // 3x
        };

        var groceriesRepoMock = new Mock<IGroceryListItemsRepository>();
        groceriesRepoMock.Setup(r => r.GetAll()).Returns(groceries);

        var productRepoMock = new Mock<IProductRepository>();
        productRepoMock.Setup(r => r.Get(It.IsAny<int>()))
            .Returns<int>(id => new Product(id, $"Product{id}", 5));

        var service = new GroceryListItemsService(groceriesRepoMock.Object, productRepoMock.Object);

        // Act
        var result = service.GetBestSellingProducts();
        
        // Debug output in een mooie tabel
        var table = new Table().RoundedBorder();
        table.AddColumn("Rank");
        table.AddColumn("Product");
        table.AddColumn("Stock");
        table.AddColumn("Verkocht");

        foreach (var p in result)
        {
            table.AddRow(p.Ranking.ToString(), p.Name, p.Stock.ToString(), p.NrOfSells.ToString());
        }

        AnsiConsole.Write(table);

        // Assert
        Assert.That(result[0].Id, Is.EqualTo(3)); // meest verkocht
        Assert.That(result[1].Id, Is.EqualTo(1)); // tweede
        Assert.That(result[2].Id, Is.EqualTo(2)); // derde
    }
}
