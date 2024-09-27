using Core.Entities;

namespace TechChallengeApi.Events
{
    public class ContactBuscaTodosEvent
    {
        public string Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Phone { get; }
        public string Ddd { get; }

        public ContactBuscaTodosEvent()
        {
            this.Id = string.Empty;
            this.Name = string.Empty;
            this.Email = string.Empty;
            this.Phone = string.Empty;  
            this.Ddd = string.Empty;
        }
    }
}
