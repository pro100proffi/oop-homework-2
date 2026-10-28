namespace OopHomework2.Models;

public readonly record struct CalculatedPrice
{
    public CalculatedPrice(decimal originalPrice, decimal discountMultiplier)
    {
        if (originalPrice < 0m)
            throw new ArgumentOutOfRangeException(nameof(originalPrice));
        if (discountMultiplier is < 0.01m or > 1m)
            throw new ArgumentOutOfRangeException(nameof(discountMultiplier));

        OriginalPrice = decimal.Round(originalPrice, 2);
        DiscountMultiplier = discountMultiplier;
        DiscountAmount = decimal.Round(OriginalPrice * (1m - discountMultiplier), 2);
    }

    public decimal OriginalPrice { get; }
    public decimal DiscountMultiplier { get; }
    public decimal DiscountAmount { get; }
    public decimal FinalPrice => OriginalPrice - DiscountAmount;

    public decimal ShowFinalPrice() => FinalPrice;
    public decimal ShowDiscountAmount() => DiscountAmount;
}
