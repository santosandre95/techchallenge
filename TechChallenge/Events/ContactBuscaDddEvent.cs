namespace TechChallengeApi.Events
{
    public class ContactBuscaDddEvent
    {
        public string Ddd { get; }
        public ContactBuscaDddEvent(string Ddd)
        {
            Ddd = Ddd;
        }
    }
}
