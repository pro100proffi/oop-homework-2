using System.Collections.ObjectModel;

namespace OopHomework2.Models;

public sealed class ShoppingCart
{
    private readonly List<CartItem> _items = [];
    private readonly ReadOnlyCollection<CartItem> _itemsView;

    public ShoppingCart(Guid id, Guid userId)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID корзины не может быть пустым.", nameof(id));
        if (userId == Guid.Empty)
            throw new ArgumentException("ID пользователя не может быть пустым.", nameof(userId));

        Id = id;
        UserId = userId;
        _itemsView = _items.AsReadOnly();
    }

    public Guid Id { get; }
    public Guid UserId { get; }
    public IReadOnlyList<CartItem> Items => _itemsView;
    public int TotalItems => _items.Sum(item => item.Quantity);

    public void Add(Guid productId, int quantity) =>
        _items.Add(new CartItem(Guid.NewGuid(), productId, quantity));

    public void Clear() => _items.Clear();

    public void Deconstruct(out Guid id, out Guid userId, out IReadOnlyList<CartItem> items) =>
        (id, userId, items) = (Id, UserId, Items);
}
