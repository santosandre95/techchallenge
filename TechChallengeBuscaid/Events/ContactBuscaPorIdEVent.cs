namespace TechChallengeBuscaid.Events
{
    public class ContactBuscaPorIdEVent
    {
        public Guid Id { get; }
        public ContactBuscaPorIdEVent(Guid id)
        {
            Id = id;
        }
    }
}
