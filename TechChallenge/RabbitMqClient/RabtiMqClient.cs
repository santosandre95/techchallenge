using Core.Entities;
using Microsoft.AspNetCore.Identity;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TechChallengeApi.RabbitMqClient
{
    public class RabtiMqClient : IRabbitMqClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabtiMqClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory()
            {
                HostName  = _configuration["RabbitMqHost"],
                Port  = Convert.ToInt32(_configuration["Port"]),
                UserName = _configuration["UserName"], 
                Password = _configuration["Password"] 


            }.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        }
        public void AddContato(Contact contact)
        {
            var mensagem = JsonSerializer.Serialize(contact);
            var body =Encoding.UTF8.GetBytes(mensagem);
            _channel.BasicPublish
                (
              
                exchange: "trigger",
                routingKey: "Add_fila",
                basicProperties: null,
                body: body
                );
        }

        public void AtualizaContato(Contact contact)
        {
            var mensagem = JsonSerializer.Serialize(contact);
            var body = Encoding.UTF8.GetBytes(mensagem);
            _channel.BasicPublish
                (

                exchange: "trigger",
                routingKey: "Add_fila",
                basicProperties: null,
                body: body
                );
        }

        public void BuscaPoID(Guid id)
        {
            var mensagem = JsonSerializer.Serialize(id);
            var body = Encoding.UTF8.GetBytes(mensagem);
            _channel.BasicPublish
                (

                exchange: "trigger",
                routingKey: "buscaId_fila",
                basicProperties: null,
                body: body
                );
        }

        public void BuscaPorDdd(string ddd)
        {
            var mensagem = JsonSerializer.Serialize(ddd);
            var body = Encoding.UTF8.GetBytes(mensagem);
            _channel.BasicPublish
                (

                exchange: "trigger",
                routingKey: "buscaTodos_fila",
                basicProperties: null,
                body: body
                );
        }

        public void Buscatodos()
        {
           
            _channel.BasicPublish
                (

                exchange: "trigger",
                routingKey: "buscaTodos_fila",
                basicProperties: null
   
                );
        }

        public void RemoveContato(Guid id)
        {
            var mensagem = JsonSerializer.Serialize(id);
            var body = Encoding.UTF8.GetBytes(mensagem);
            _channel.BasicPublish
                (

                exchange: "trigger",
                routingKey: "Delete_fila",
                basicProperties: null,
                body: body
                );
        }
    }
}
