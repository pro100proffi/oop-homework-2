namespace OopHomework2.Models;

public sealed class CartItem
{
    public CartItem(Guid id, Guid productId, int quantity)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID позиции не может быть пустым.", nameof(id));
        if (productId == Guid.Empty)
            throw new ArgumentException("ID товара не может быть пустым.", nameof(productId));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть больше нуля.");

        Id = id;
        ProductId = productId;
        Quantity = quantity;
    }

    public Guid Id { get; }
    public Guid ProductId { get; }
    public int Quantity { get; }

    public void Deconstruct(out Guid id, out Guid productId, out int quantity) =>
        (id, productId, quantity) = (Id, ProductId, Quantity);
}
