namespace TechChallengeApi.Events
{
    public class BuscaDddEvent
    {
        public  string Ddd { get; }

        public BuscaDddEvent(string Ddd)
        {
            this.Ddd = Ddd;
        }
    }
}
