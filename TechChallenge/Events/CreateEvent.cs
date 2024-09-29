using Core.Entities;

namespace TechChallengeApi.Events
{
    public class CreateEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }
        public string Ddd { get; }

        public CreateEvent(Contact contact)
        {
            Id = contact.Id;
            Name = contact.Name;
            Email = contact.Email;
            Phone = contact.Phone;
            Ddd = contact.Ddd;
        }
    }
}
