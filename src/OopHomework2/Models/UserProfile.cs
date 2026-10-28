using System.Collections.ObjectModel;

namespace OopHomework2.Models;

public sealed class UserProfile
{
    private readonly List<Contact> _contacts = [];
    private readonly ReadOnlyCollection<Contact> _contactsView;

    public UserProfile(User user)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
        _contactsView = _contacts.AsReadOnly();
    }

    public User User { get; }
    public IReadOnlyList<Contact> Contacts => _contactsView;

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
}
