using Core.Entities;

namespace Application.Applications.Interfaces
{
    public interface IContactApplication
    {
        Task AddAsync(Contact contact);
        Task UpdateAsync(Contact contact);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact?> GetAsync(Guid id);
        Task<IEnumerable<Contact>> GetContactsByDddAsync(string ddd);
        Task<bool> CheckIfExistsAsync(Guid id);
    }
}
