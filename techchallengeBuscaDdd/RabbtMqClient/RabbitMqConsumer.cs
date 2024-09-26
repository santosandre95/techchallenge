using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TechChallengeBuscaDdd.EventProcess;

namespace TechChallengeBuscaDdd.RabbtMqClient
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;
        private readonly string _nomeFila;
        private readonly IContactProcess _prossaEvento;

        public RabbitMqConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqHost"],
                Port = Convert.ToInt32(_configuration["Port"]),
                UserName = _configuration["UserName"],
                Password = _configuration["Password"]


            }.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _nomeFila = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _nomeFila, exchange: "trigger", routingKey: "buscaDdd_fila");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body;
                var menssagem = Encoding.UTF8.GetString(body.ToArray());

                _prossaEvento.Process(menssagem);

            };
            _channel.BasicConsume(queue: _nomeFila, autoAck: true, consumer: consumer);
            return Task.CompletedTask;

        }
    }
}
