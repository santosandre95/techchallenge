using Core.Entities;

namespace TechChallengeApi.Events
{
    public class UpdateEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }
        public UpdateEvent(Contact contact)
        {
            Id = contact.Id;
            Name = contact.Name;
            Email = contact.Email;
            Phone = contact.Phone;
        }

    }
}
