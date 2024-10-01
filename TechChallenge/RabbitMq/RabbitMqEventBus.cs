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

   
        public async Task PublishCreated(CreateEvent createdEvent)
        {
            await Publish("contact_created", createdEvent);
        }
        public async Task PublishDeleted(DeleteEvent deleteEvent)
        {
           await Publish("contact_deleted", deleteEvent);
        }

        public async Task PublishUpdated(UpdateEvent  updateEvent)
        {
            await Publish("contact_updated", updateEvent);
        }
        public async Task PublishBuscaId(BuscaIdEvent buscaidEvent)
        {
           await Publish("contact_buscaid", buscaidEvent);
        }
        public async Task PublishBuscaDdd(BuscaDddEvent buscaDddEvent)
        {
            await Publish("contact_buscaddd", buscaDddEvent);
        }
        public async Task PublishBuscaTodos(BuscaTodosEvent buscaTodosEvent)
        {
            await Publish("contact_buscatodos", buscaTodosEvent);
        }

        private async Task Publish<T>(string routingKey, T eventMessage)
        {
            var message = System.Text.Json.JsonSerializer.Serialize(eventMessage);
            var body = Encoding.UTF8.GetBytes(message);

            await Task.Run(() =>
                _channel.BasicPublish(exchange: "", routingKey: routingKey, basicProperties: null, body: body)
            );
        }
    }
}
