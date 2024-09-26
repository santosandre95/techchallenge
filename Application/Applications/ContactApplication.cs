using Application.Applications.Interfaces;
using Core.Entities;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Repositories.Interface;

namespace Application.Applications
{
    public class ContactApplication : IContactApplication
    {
        protected readonly IContactRepository _contactRepository;
        protected readonly IValidator<Contact> _contactValidator;
        private IContactRepository object1;
        private IValidator<Contact> object2;

        public ContactApplication(IContactRepository contactRepository, IValidator<Contact> contactValidator, global::MassTransit.IBus @object)
        {
            _contactRepository = contactRepository;
            _contactValidator = contactValidator;
        }

        public ContactApplication(IContactRepository object1, IValidator<Contact> object2)
        {
            this.object1 = object1;
            this.object2 = object2;
        }

        public async Task<Contact?> GetAsync(Guid id)
        {
            if (!await _contactRepository.CheckIfExistsAsync(id))
            {
                throw new KeyNotFoundException("Contato não encontrado.");
            }

            return await _contactRepository.GetAsync(id);
        }

        public async Task AddAsync(Contact contact)
        {
            ValidationResult validationResult = await _contactValidator.ValidateAsync(contact);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _contactRepository.AddAsync(contact);
        }

        public async Task UpdateAsync(Contact contact)
        {
            ValidationResult validationResult = await _contactValidator.ValidateAsync(contact);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (!await _contactRepository.CheckIfExistsAsync(contact.Id))
            {
                throw new KeyNotFoundException("Contato não encontrado.");
            }

            await _contactRepository.UpdateAsync(contact);
        }

        public async Task DeleteAsync(Guid id)
        {
            var contact = await _contactRepository.GetAsync(id);
            if (contact is null)
            {
                throw new KeyNotFoundException("Contato não encontrado.");
            }

            await _contactRepository.DeleteAsync(contact);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _contactRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Contact>> GetContactsByDddAsync(string ddd)
        {
            return await _contactRepository.GetContactsByDddAsync(ddd);
        }

        public async Task<bool> CheckIfExistsAsync(Guid id)
        {
            return await _contactRepository.CheckIfExistsAsync(id);
        }
    }
}
