using Core.Entities;

namespace TechChallengeApi.RabbitMqClient
{
    public interface IRabbitMqClient
    {
        void AddContato(Contact contact);
    }
}
