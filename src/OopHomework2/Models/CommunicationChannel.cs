namespace OopHomework2.Models;

public sealed class CommunicationChannel
{
    private string _name;

    public CommunicationChannel(Guid id, string name)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID канала не может быть пустым.", nameof(id));

        Id = id;
        _name = ValidateName(name);
    }

    public Guid Id { get; }

    public string Name
    {
        get => _name;
        set => _name = ValidateName(value);
    }

    public void Deconstruct(out Guid id, out string name) => (id, name) = (Id, Name);

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название канала не может быть пустым.", nameof(name));
        return name.Trim();
    }
}
