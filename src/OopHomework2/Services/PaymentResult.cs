using OopHomework2.Models;

namespace OopHomework2.Services;

public sealed record PaymentResult(
    bool Success,
    string Message,
    CalculatedPrice Price,
    decimal BonusSpent,
    decimal MoneySpent,
    decimal MissingMoney,
    BuyerBalanceState BalanceAfterPayment);
