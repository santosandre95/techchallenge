using Core.Entities;

namespace TechChallengeApi.Events
{
    public class ContactUpdateEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }

        public ContactUpdateEvent(Contact contact)
        {
            Id = contact.Id;
            Name = contact.Name;
            Email = contact.Email;
            Phone = contact.Phone;
        }
    }
}
