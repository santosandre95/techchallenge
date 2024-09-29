namespace TechChallengeApi.Events
{
    public class DeleteEvent
    {
        public Guid Id { get; }
        public DeleteEvent(Guid id)
        {
            Id = id;
        }
    }
}
