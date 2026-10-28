using OopHomework2.Models;
using OopHomework2.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var profileService = new UserProfileService(new Pbkdf2PasswordHasher());
var profile = profileService.CreateProfile(
    "Гаврилов Артем Алексеевич",
    "gavrilov.aa",
    "StudyPassword2026");

var email = new CommunicationChannel(Guid.NewGuid(), "Электронная почта");
var messenger = new CommunicationChannel(Guid.NewGuid(), "Мессенджер");
var channels = new Dictionary<Guid, CommunicationChannel>
{
    [email.Id] = email,
    [messenger.Id] = messenger
};

profileService.AddContact(profile, email, "artem.gavrilov@example.com");
profileService.AddContact(profile, messenger, "@gavrilov_aa");

Console.WriteLine(profileService.GetBriefStatistics(profile));
foreach (var contact in profile.Contacts)
    Console.WriteLine($"- {channels[contact.ChannelId].Name}: {contact.Value}");
