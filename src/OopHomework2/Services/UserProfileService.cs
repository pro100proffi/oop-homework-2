using OopHomework2.Models;

namespace OopHomework2.Services;

public sealed class UserProfileService
{
    private readonly Dictionary<Guid, UserProfile> _profiles = [];
    private readonly IPasswordHasher _passwordHasher;

    public UserProfileService(IPasswordHasher passwordHasher) =>
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));

    public UserProfile CreateProfile(string fullName, string login, string password)
    {
        if (_profiles.Values.Any(profile =>
                string.Equals(profile.User.Login, login, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Профиль с таким логином уже существует.");
        }

        var user = new User(Guid.NewGuid(), fullName, login, _passwordHasher.Hash(password));
        var profile = new UserProfile(user);
        _profiles.Add(user.Id, profile);
        return profile;
    }

    public void AddContact(UserProfile profile, CommunicationChannel channel, string value)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(channel);
        profile.AddContact(new Contact(Guid.NewGuid(), channel.Id, value));
    }

    public void RenameUser(UserProfile profile, string newFullName)
    {
        ArgumentNullException.ThrowIfNull(profile);
        profile.User.FullName = newFullName;
    }

    public string GetBriefStatistics(UserProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        (var fullName, var login, var contactCount) = profile;
        return $"Пользователь: {fullName}; логин: {login}; контактов: {contactCount}.";
    }
}
