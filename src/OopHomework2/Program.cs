using OopHomework2.Models;
using OopHomework2.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var profileService = new UserProfileService(new Pbkdf2PasswordHasher());
var profile = profileService.CreateProfile("Покупатель", "buyer", "StudyPassword2026");

var email = new CommunicationChannel(Guid.NewGuid(), "Электронная почта");
profileService.AddContact(profile, email, "buyer@example.com");
Console.WriteLine(profileService.GetBriefStatistics(profile));
Console.WriteLine($"Контакт: {email.Name} — {profile.Contacts[0].Value}");

var plate = new Product(Guid.NewGuid(), "Тарелка", 1_000m);
var cutlery = new Product(Guid.NewGuid(), "Приборы", 2_000m);
var coffeeMachine = new Product(Guid.NewGuid(), "Кофемашина", 25_000m);
IReadOnlyDictionary<Guid, Product> products = new Dictionary<Guid, Product>
{
    [plate.Id] = plate,
    [cutlery.Id] = cutlery,
    [coffeeMachine.Id] = coffeeMachine
};

profile.CreditMoney(10_000m);
profile.CreditBonuses(1_000m);
profile.SetMaxBonusPaymentShare(0.10m);
profile.SetDiscountMultiplier(0.95m);

var pricingService = new CartPricingService();
var purchaseService = new PurchaseService(pricingService);
var cart = new ShoppingCart(Guid.NewGuid(), profile.User.Id);

var sameCart = cart;
sameCart.Add(plate.Id, 1);
Console.WriteLine(
    $"Копирование ссылки на корзину: первая переменная = {cart.TotalItems}, " +
    $"вторая = {sameCart.TotalItems}");
cart.Clear();

var originalBalance = profile.GetBalanceState();
var changedBalance = originalBalance with { Money = originalBalance.Money + 500m };
Console.WriteLine(
    $"Копирование состояния: оригинал = {originalBalance.Money:F2} УЕ, " +
    $"копия = {changedBalance.Money:F2} УЕ");

cart.Add(plate.Id, 3);
cart.Add(cutlery.Id, 2);
var firstPrice = pricingService.Calculate(cart, products, profile.DiscountMultiplier);
Console.WriteLine($"Исходная цена: {firstPrice.OriginalPrice:F2} УЕ");
Console.WriteLine($"Цена со скидкой: {firstPrice.ShowFinalPrice():F2} УЕ");
Console.WriteLine($"Размер скидки: {firstPrice.ShowDiscountAmount():F2} УЕ");

var firstPayment = purchaseService.TryPay(profile, cart, products);
PrintPayment(firstPayment, cart, profile);

var realBalance = firstPayment.BalanceAfterPayment;
var balanceCopy = realBalance with { Money = realBalance.Money + 500m };
Console.WriteLine(
    $"with: исходный баланс {realBalance.Money:F2} УЕ, " +
    $"в копии {balanceCopy.Money:F2} УЕ");

cart.Add(coffeeMachine.Id, 1);
var moneyBeforeFailure = profile.MoneyBalance;
var bonusBeforeFailure = profile.BonusBalance;
var itemsBeforeFailure = cart.TotalItems;

var failedPayment = purchaseService.TryPay(profile, cart, products);
PrintPayment(failedPayment, cart, profile);
Console.WriteLine(
    $"После отказа состояние сохранено: " +
    $"деньги {moneyBeforeFailure == profile.MoneyBalance}, " +
    $"бонусы {bonusBeforeFailure == profile.BonusBalance}, " +
    $"товары {itemsBeforeFailure == cart.TotalItems}");

profile.CreditMoney(failedPayment.MissingMoney);
Console.WriteLine($"Баланс пополнен на {failedPayment.MissingMoney:F2} УЕ. Повторяем оплату.");
var retryPayment = purchaseService.TryPay(profile, cart, products);
PrintPayment(retryPayment, cart, profile);

static void PrintPayment(PaymentResult result, ShoppingCart cart, UserProfile profile)
{
    Console.WriteLine(result.Message);
    Console.WriteLine(
        $"Списано: {result.BonusSpent:F2} бонусов и {result.MoneySpent:F2} УЕ. " +
        $"В корзине: {cart.TotalItems}, покупок в профиле: {profile.Purchases.Count}.");
    Console.WriteLine(
        $"Баланс: {profile.MoneyBalance:F2} УЕ и {profile.BonusBalance:F2} бонусов.");
}
