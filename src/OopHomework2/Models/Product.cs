namespace OopHomework2.Models;

public sealed class Product
{
    public Product(Guid id, string name, decimal unitPrice)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID товара не может быть пустым.", nameof(id));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название товара не может быть пустым.", nameof(name));
        if (unitPrice <= 0m)
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Цена должна быть больше нуля.");

        Id = id;
        Name = name.Trim();
        UnitPrice = decimal.Round(unitPrice, 2);
    }

    public Guid Id { get; }
    public string Name { get; }
    public decimal UnitPrice { get; }

    public void Deconstruct(out Guid id, out string name, out decimal unitPrice) =>
        (id, name, unitPrice) = (Id, Name, UnitPrice);
}
