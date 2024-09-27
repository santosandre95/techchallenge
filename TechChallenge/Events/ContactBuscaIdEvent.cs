namespace TechChallengeApi.Events
{

    namespace TechChallengeApi.Events
    {
        public class ContactBuscaIdEvent
        {
            public Guid Id { get; }
            public ContactBuscaIdEvent(Guid id)
            {
                Id = id;
            }
        }
    }
}
