using Core.Entities;

namespace TechChallengeApi.RabbitMqClient
{
    public interface IRabbitMqClient
    {
        void AddContato(Contact contact);
        void RemoveContato(Guid id);
        void BuscaPoID(Guid id);
        void Buscatodos();
        void AtualizaContato(Contact contact);
        void BuscaPorDdd(string ddd); 

    }
}
