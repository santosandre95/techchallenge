namespace TechChallengeApi.RabbitMqEvents
{
    public class RabbitMqSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string GetConnectionString()
        {
            return $"amqp://{UserName}:{Password}@{Host}:{Port}";
        }
    }
}
