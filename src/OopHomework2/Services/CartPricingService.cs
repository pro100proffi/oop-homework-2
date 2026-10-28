using OopHomework2.Models;

namespace OopHomework2.Services;

public sealed class CartPricingService
{
    public CalculatedPrice Calculate(
        ShoppingCart cart,
        IReadOnlyDictionary<Guid, Product> products,
        decimal discountMultiplier)
    {
        ArgumentNullException.ThrowIfNull(cart);
        ArgumentNullException.ThrowIfNull(products);

        var originalPrice = CalculateOriginalPrice(cart.Items, products);
        return new CalculatedPrice(originalPrice, discountMultiplier);
    }

    private static decimal CalculateOriginalPrice(
        IEnumerable<CartItem> items,
        IReadOnlyDictionary<Guid, Product> products)
    {
        decimal total = 0m;
        foreach (var item in items)
            total += FindProduct(item.ProductId, products).UnitPrice * item.Quantity;
        return decimal.Round(total, 2);
    }

    private static Product FindProduct(
        Guid productId,
        IReadOnlyDictionary<Guid, Product> products)
    {
        if (!products.TryGetValue(productId, out var product))
            throw new KeyNotFoundException($"Товар {productId} не найден.");
        return product;
    }
}
