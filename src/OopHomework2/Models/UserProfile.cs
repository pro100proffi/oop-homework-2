using System.Collections.ObjectModel;

namespace OopHomework2.Models;

public sealed class UserProfile
{
    private readonly List<Contact> _contacts = [];
    private readonly ReadOnlyCollection<Contact> _contactsView;
    private readonly List<CartItem> _purchases = [];
    private readonly ReadOnlyCollection<CartItem> _purchasesView;

    public UserProfile(User user)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
        _contactsView = _contacts.AsReadOnly();
        _purchasesView = _purchases.AsReadOnly();
        DiscountMultiplier = 1m;
    }

    public User User { get; }
    public IReadOnlyList<Contact> Contacts => _contactsView;
    public decimal MoneyBalance { get; private set; }
    public decimal BonusBalance { get; private set; }
    public decimal DiscountMultiplier { get; private set; }
    public decimal MaxBonusPaymentShare { get; private set; }
    public IReadOnlyList<CartItem> Purchases => _purchasesView;

    public void AddContact(Contact contact)
    {
        ArgumentNullException.ThrowIfNull(contact);

        var duplicateExists = _contacts.Any(existing =>
            existing.ChannelId == contact.ChannelId &&
            string.Equals(existing.Value, contact.Value, StringComparison.OrdinalIgnoreCase));

        if (duplicateExists)
            throw new InvalidOperationException("Такой контакт уже добавлен в профиль.");

        _contacts.Add(contact);
    }

    public bool RemoveContact(Guid contactId)
    {
        if (contactId == Guid.Empty)
            throw new ArgumentException("ID контакта не может быть пустым.", nameof(contactId));

        var contact = _contacts.FirstOrDefault(item => item.Id == contactId);
        return contact is not null && _contacts.Remove(contact);
    }

    public void Deconstruct(out string fullName, out string login, out int contactCount) =>
        (fullName, login, contactCount) = (User.FullName, User.Login, _contacts.Count);

    public void CreditMoney(decimal amount)
    {
        EnsurePositive(amount, nameof(amount));
        MoneyBalance = decimal.Round(MoneyBalance + amount, 2);
    }

    public void CreditBonuses(decimal amount)
    {
        EnsurePositive(amount, nameof(amount));
        BonusBalance = decimal.Round(BonusBalance + amount, 2);
    }

    public void SetDiscountMultiplier(decimal multiplier)
    {
        if (multiplier is < 0.01m or > 1m)
            throw new ArgumentOutOfRangeException(nameof(multiplier));
        DiscountMultiplier = multiplier;
    }

    public void SetMaxBonusPaymentShare(decimal share)
    {
        if (share is < 0m or > 0.5m)
            throw new ArgumentOutOfRangeException(nameof(share));
        MaxBonusPaymentShare = share;
    }

    public BuyerBalanceState GetBalanceState() =>
        new(BonusBalance, MoneyBalance, MaxBonusPaymentShare);

    internal void ApplySuccessfulPayment(
        BuyerBalanceState newState,
        IEnumerable<CartItem> purchases)
    {
        ArgumentNullException.ThrowIfNull(purchases);
        MoneyBalance = newState.Money;
        BonusBalance = newState.Bonuses;
        MaxBonusPaymentShare = newState.MaxBonusPaymentShare;
        _purchases.AddRange(purchases);
    }

    private static void EnsurePositive(decimal amount, string parameterName)
    {
        if (amount <= 0m)
            throw new ArgumentOutOfRangeException(parameterName, "Сумма должна быть больше нуля.");
    }
}
