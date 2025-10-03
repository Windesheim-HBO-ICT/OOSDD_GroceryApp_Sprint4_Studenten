using Grocery.Core.Models;

namespace TestCore.Builders;

public class GroceryListItemBuilder
{
    private int _id = 1;
    private int _productId = 1;
    private int _userId = 1;
    private int _listId = 1;

    public GroceryListItemBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public GroceryListItemBuilder WithProductId(int productId)
    {
        _productId = productId;
        return this;
    }

    public GroceryListItemBuilder WithUserId(int userId)
    {
        _userId = userId;
        return this;
    }

    public GroceryListItemBuilder WithListId(int listId)
    {
        _listId = listId;
        return this;
    }

    public GroceryListItem Build()
    {
        return new GroceryListItem(_id, _productId, _userId, _listId);
    }

    public static List<GroceryListItem> CreateMany(params (int productId, int count)[] products)
    {
        var items = new List<GroceryListItem>();
        var id = 1;
        foreach (var (productId, count) in products)
        {
            for (var i = 0; i < count; i++)
            {
                items.Add(new GroceryListItem(id++, 1,productId, 100));
            }
        }
        return items;
    }
}