using Core.Entities;
using Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Contact>> GetContactsByDddAsync(string ddd)
        {
            return await _dbSet.Where(x => x.Ddd == ddd).ToListAsync();
        }
    }
}
