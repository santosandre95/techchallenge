using Core.Entities;

namespace TechChallengeAdd.Events
{
    public class ContactCreateEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }


        public ContactCreateEvent(Contact contact)
        {
            Id = contact.Id;
            Name = contact.Name;
            Email = contact.Email;
            Phone = contact.Phone;
        }
    }
}
