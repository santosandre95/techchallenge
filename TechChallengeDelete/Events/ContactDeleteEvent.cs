namespace TechChallengeDelete.Events
{
    public class ContactDeleteEvent
    {
        public Guid Id { get; }
        public ContactDeleteEvent(Guid id)
        {
            Id = id;
        }
    }
}
