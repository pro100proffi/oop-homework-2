namespace OopHomework2.Models;

public sealed class User
{
    private string _fullName;
    private string _login;
    private string _passwordHash;

    public User(Guid id, string fullName, string login, string passwordHash)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID пользователя не может быть пустым.", nameof(id));

        Id = id;
        _fullName = ValidateText(fullName, nameof(fullName));
        _login = ValidateText(login, nameof(login));
        _passwordHash = ValidateText(passwordHash, nameof(passwordHash));
    }

    public Guid Id { get; }

    public string FullName
    {
        get => _fullName;
        set => _fullName = ValidateText(value, nameof(value));
    }

    public string Login
    {
        get => _login;
        set => _login = ValidateText(value, nameof(value));
    }

    public string PasswordHash => _passwordHash;

    public void ChangePasswordHash(string passwordHash) =>
        _passwordHash = ValidateText(passwordHash, nameof(passwordHash));

    public void Deconstruct(
        out Guid id,
        out string fullName,
        out string login,
        out string passwordHash) =>
        (id, fullName, login, passwordHash) = (Id, FullName, Login, PasswordHash);

    private static string ValidateText(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Значение не может быть пустым.", parameterName);
        return value.Trim();
    }
}
