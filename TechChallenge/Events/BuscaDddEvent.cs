namespace TechChallengeApi.Events
{
    public class BuscaDddEvent
    {
        public string Ddd { get; }

        public BuscaDddEvent(string Ddd)
        {
            Ddd = Ddd;
        }
    }
}
