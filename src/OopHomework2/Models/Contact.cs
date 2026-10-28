namespace OopHomework2.Models;

public sealed class Contact
{
    private string _value;

    public Contact(Guid id, Guid channelId, string value)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID контакта не может быть пустым.", nameof(id));
        if (channelId == Guid.Empty)
            throw new ArgumentException("ID канала не может быть пустым.", nameof(channelId));

        Id = id;
        ChannelId = channelId;
        _value = ValidateValue(value);
    }

    public Guid Id { get; }
    public Guid ChannelId { get; }

    public string Value
    {
        get => _value;
        set => _value = ValidateValue(value);
    }

    public void Deconstruct(out Guid id, out Guid channelId, out string value) =>
        (id, channelId, value) = (Id, ChannelId, Value);

    private static string ValidateValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Значение контакта не может быть пустым.", nameof(value));
        return value.Trim();
    }
}
