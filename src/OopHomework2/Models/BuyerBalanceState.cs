namespace OopHomework2.Models;

public record struct BuyerBalanceState
{
    private decimal _bonuses;
    private decimal _money;
    private decimal _maxBonusPaymentShare;

    public BuyerBalanceState(decimal bonuses, decimal money, decimal maxBonusPaymentShare)
    {
        _bonuses = 0m;
        _money = 0m;
        _maxBonusPaymentShare = 0m;
        Bonuses = bonuses;
        Money = money;
        MaxBonusPaymentShare = maxBonusPaymentShare;
    }

    public decimal Bonuses
    {
        readonly get => _bonuses;
        init
        {
            if (value < 0m)
                throw new ArgumentOutOfRangeException(nameof(value), "Бонусный баланс не может быть отрицательным.");
            _bonuses = decimal.Round(value, 2);
        }
    }

    public decimal Money
    {
        readonly get => _money;
        init
        {
            if (value < 0m)
                throw new ArgumentOutOfRangeException(nameof(value), "Денежный баланс не может быть отрицательным.");
            _money = decimal.Round(value, 2);
        }
    }

    public decimal MaxBonusPaymentShare
    {
        readonly get => _maxBonusPaymentShare;
        init
        {
            if (value is < 0m or > 0.5m)
                throw new ArgumentOutOfRangeException(nameof(value), "Доля оплаты бонусами должна быть от 0 до 0,5.");
            _maxBonusPaymentShare = value;
        }
    }
}
