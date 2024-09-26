using Application.Applications.Interfaces;
using Infrastructure.Repositories;
using System.Text.Json;

namespace TechChallengeDelete.EventProcess
{
    public class ContactProcess : IContactProcess
    {
        private readonly IContactApplication _contactApplication;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ContactProcess(IContactApplication contactApplication, IServiceScopeFactory serviceScopeFactory)
        {
            _contactApplication = contactApplication;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public void Process(string mensagem)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var contactRepository = scope.ServiceProvider.GetRequiredService<ContactRepository>();
            var id = JsonSerializer.Deserialize<Guid>(mensagem);
            var addContact = _contactApplication.DeleteAsync(id);
        }
    }
}
