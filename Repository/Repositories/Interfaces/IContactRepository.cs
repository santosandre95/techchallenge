using Core.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Interface
{
    public interface IContactRepository : IBaseRepository<Contact>
    {
        Task<IEnumerable<Contact>> GetContactsByDddAsync(string ddd);
    }
}
