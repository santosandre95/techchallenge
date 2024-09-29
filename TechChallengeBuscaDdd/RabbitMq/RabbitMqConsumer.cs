using Application.Applications.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace TechChallengeBuscaDdd.RabbitMq
{

    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqConsumer(IOptions<RabbitMqSettings> settings, IServiceProvider serviceProvider)
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
            _serviceProvider = serviceProvider;

            _channel.QueueDeclare(queue: "contact_buscaddd", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Manssagem Recebdia: {message}");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var contactApplication = scope.ServiceProvider.GetRequiredService<IContactApplication>();
                    var contact = JsonConvert.DeserializeObject<Contact>(message);
                    if (contact != null)
                    {
                        await contactApplication.GetContactsByDddAsync(contact.Ddd);
                        Console.WriteLine($"Contato : {contact}");
                    }
                }
            };

            _channel.BasicConsume(queue: "contact_buscaddd", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
