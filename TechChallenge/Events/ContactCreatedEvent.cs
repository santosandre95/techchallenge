using Core.Entities;

namespace TechChallengeApi.Events
{
    public class ContactCreatedEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }
        public string Ddd { get; }

        public ContactCreatedEvent(Contact contact)
        {
            Id = contact.Id;
            Name = contact.Name;
            Email = contact.Email;
            Phone = contact.Phone;
            Ddd = contact.Ddd;  
        }
    }
}
