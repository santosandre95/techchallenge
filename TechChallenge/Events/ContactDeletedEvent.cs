namespace TechChallengeApi.Events
{
    public class ContactDeletedEvent
    {

        public Guid Id { get; }

        public ContactDeletedEvent(Guid id)
        {
            Id = id;
        }
    }

}

