using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using TechChallengeApi.Events;
using TechChallengeApi.Events.TechChallengeApi.Events;

namespace TechChallengeApi.RabbitMqEvents
{
    public class RabbitMqEventBus
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqEventBus(IOptions<RabbitMqSettings> settings)
        {
            var factory = new ConnectionFactory()
            {
                HostName = settings.Value.Host,
                Port = int.Parse(settings.Value.Port),
                UserName = settings.Value.UserName,
                Password = settings.Value.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();


            _channel.QueueDeclare(queue: "contact_created", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "contact_deleted", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "contact_updated", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "contact_buscaid", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "contact_buscatodos", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: "contact_buscaddd", durable: false, exclusive: false, autoDelete: false, arguments: null);

        }
        public void PublishContactCreated(ContactCreatedEvent contactCreatedEvent)
        {
            Publish("contact_created", contactCreatedEvent);
        }

        public void PublishContactDeleted(ContactDeletedEvent contactDeletedEvent)
        {
            Publish("contact_deleted", contactDeletedEvent);
        }

        public void PublishContactUpdated(ContactUpdateEvent contactUpdatedEvent)
        {
            Publish("contact_updated", contactUpdatedEvent);
        }
        public void PublishContactBuscaId(ContactBuscaIdEvent contactBuscaidEvent)
        {
            Publish("contact_buscaid", contactBuscaidEvent);
        }
        public void PublishContactBuscaDdd(ContactBuscaDddEvent contactBuscaDddEvent)
        {
            Publish("contact_buscaddd", contactBuscaDddEvent);
        }
        public void PublishContactBuscaTodos(ContactBuscaTodosEvent contactBuscaTodosEvent)
        {
            Publish("contact_buscatodos", contactBuscaTodosEvent);
        }
        private void Publish<T>(string routingKey, T eventMessage)
        {
            var message = System.Text.Json.JsonSerializer.Serialize(eventMessage);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "", routingKey: routingKey, basicProperties: null, body: body);
        }
    }
}

