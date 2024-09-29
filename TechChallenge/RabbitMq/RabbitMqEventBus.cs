using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using TechChallengeApi.Events;

namespace TechChallengeApi.RabbitMq
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
                Port = Convert.ToInt32(settings.Value.Port),
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

   
        public void PublishCreated(CreateEvent createdEvent)
        {
            Publish("contact_created", createdEvent);
        }
        public void PublishDeleted(DeleteEvent deleteEvent)
        {
            Publish("contact_deleted", deleteEvent);
        }

        public void PublishUpdated(UpdateEvent  updateEvent)
        {
            Publish("contact_updated", updateEvent);
        }
        public void PublishBuscaId(BuscaIdEvent buscaidEvent)
        {
            Publish("contact_buscaid", buscaidEvent);
        }
        public void PublishBuscaDdd(BuscaDddEvent buscaDddEvent)
        {
            Publish("contact_buscaddd", buscaDddEvent);
        }
        public void PublishBuscaTodos(BuscaTodosEvent buscaTodosEvent)
        {
            Publish("contact_buscatodos", buscaTodosEvent);
        }

        private void Publish<T>(string routingKey, T eventMessage)
        {
            var message = System.Text.Json.JsonSerializer.Serialize(eventMessage);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "", routingKey: routingKey, basicProperties: null, body: body);
        }
    }
}
