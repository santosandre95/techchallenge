﻿using Application.Applications.Interfaces;
using Core.Entities;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace TechChallengeBuscaId.RabbitMqEvents
{
    public class RabbitMqEventConsumer
    {
        private readonly IModel _channel;
        private readonly IContactApplication _contactApplication;

        public RabbitMqEventConsumer(IOptions<RabbitMqSettings> settings, IContactApplication contactApplication)
        {
            var factory = new ConnectionFactory()
            {
                HostName = settings.Value.Host,
                Port = int.Parse(settings.Value.Port),
                UserName = settings.Value.UserName,
                Password = settings.Value.Password
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _contactApplication = contactApplication;
            StartConsuming();
        }

        private void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await RegisterContact(message);
            };

            _channel.BasicConsume(queue: "contact_buscaid", autoAck: true, consumer: consumer);
        }

        private async Task RegisterContact(string message)
        {
            try
            {

                var contact = JsonConvert.DeserializeObject<Contact>(message);
                if (contact != null)
                {
                    await _contactApplication.GetAsync(contact.Id);
                    Console.WriteLine($"Contato : {contact}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Contato não encontrado: {ex.Message}");
            }
        }
    }

}
