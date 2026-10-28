using OopHomework2.Models;

namespace OopHomework2.Services;

public sealed class PurchaseService
{
    private readonly CartPricingService _pricingService;

    public PurchaseService(CartPricingService pricingService) =>
        _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));

    public PaymentResult TryPay(
        UserProfile profile,
        ShoppingCart cart,
        IReadOnlyDictionary<Guid, Product> products)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(cart);
        ArgumentNullException.ThrowIfNull(products);

        if (profile.User.Id != cart.UserId)
            throw new InvalidOperationException("Корзина принадлежит другому пользователю.");

        var price = _pricingService.Calculate(cart, products, profile.DiscountMultiplier);
        var before = profile.GetBalanceState();

        if (cart.Items.Count == 0)
            return Failure("Корзина пуста.", price, before, 0m);

        var allowedBonus = decimal.Round(price.FinalPrice * before.MaxBonusPaymentShare, 2);
        var bonusToSpend = Math.Min(before.Bonuses, allowedBonus);
        var moneyToSpend = price.FinalPrice - bonusToSpend;

        if (before.Money < moneyToSpend)
        {
            var missing = decimal.Round(moneyToSpend - before.Money, 2);
            return Failure($"Недостаточно средств. Не хватает {missing:F2} УЕ.", price, before, missing);
        }

        var after = before with
        {
            Bonuses = before.Bonuses - bonusToSpend,
            Money = before.Money - moneyToSpend
        };

        var purchasedItems = cart.Items.ToArray();
        profile.ApplySuccessfulPayment(after, purchasedItems);
        cart.Clear();

        return new PaymentResult(
            true,
            "Оплата прошла успешно.",
            price,
            bonusToSpend,
            moneyToSpend,
            0m,
            after);
    }

    private static PaymentResult Failure(
        string message,
        CalculatedPrice price,
        BuyerBalanceState unchangedBalance,
        decimal missingMoney) =>
        new(false, message, price, 0m, 0m, missingMoney, unchangedBalance);
}
