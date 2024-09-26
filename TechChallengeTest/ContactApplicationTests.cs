using Moq;
using Core.Entities;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Repositories.Interface;
using Application.Applications;

public class ContactApplicationTests
{
    private readonly Mock<IContactRepository> _mockContactRepository;
    private readonly Mock<IValidator<Contact>> _mockContactValidator;
    private readonly ContactApplication _contactApplication;

    public ContactApplicationTests()
    {
        _mockContactRepository = new Mock<IContactRepository>();
        _mockContactValidator = new Mock<IValidator<Contact>>();
        _contactApplication = new ContactApplication(_mockContactRepository.Object, _mockContactValidator.Object);
    }

    [Fact]
    public async Task GetAsync_ShouldThrowKeyNotFoundException_WhenContactDoesNotExist()
    {
        var contactId = Guid.NewGuid();
        _mockContactRepository.Setup(repo => repo.CheckIfExistsAsync(contactId)).ReturnsAsync(false);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _contactApplication.GetAsync(contactId));
    }

    [Fact]
    public async Task GetAsync_ShouldReturnContact_WhenContactExists()
    {
        var contactId = Guid.NewGuid();
        var contact = new Contact { Id = contactId, Name = "Test", Email = "test@example.com", Phone = "12345678", Ddd = "11" };
        _mockContactRepository.Setup(repo => repo.CheckIfExistsAsync(contactId)).ReturnsAsync(true);
        _mockContactRepository.Setup(repo => repo.GetAsync(contactId)).ReturnsAsync(contact);
        var result = await _contactApplication.GetAsync(contactId);
        Assert.Equal(contact, result);
    }

    [Theory]
    [InlineData("", "invalid-email", "123", "")]
    [InlineData("Valid Name", "invalid-email", "123", "11")]
    [InlineData("Valid Name", "valid@example.com", "123", "")]
    public async Task AddAsync_ShouldThrowValidationException_WhenContactIsInvalid(string name, string email, string phone, string ddd)
    {
        var contact = new Contact { Name = name, Email = email, Phone = phone, Ddd = ddd };
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required"),
            new ValidationFailure("Email", "Email is invalid"),
            new ValidationFailure("Phone", "Phone is invalid"),
            new ValidationFailure("Ddd", "Ddd is invalid")
        });
        _mockContactValidator.Setup(validator => validator.ValidateAsync(contact, default)).ReturnsAsync(validationResult);
        await Assert.ThrowsAsync<ValidationException>(() => _contactApplication.AddAsync(contact));
    }

    [Fact]
    public async Task AddAsync_ShouldAddContact_WhenContactIsValid()
    {
        var contact = new Contact { Name = "Valid Name", Email = "valid@example.com", Phone = "12345678", Ddd = "11" };
        var validationResult = new ValidationResult();
        _mockContactValidator.Setup(validator => validator.ValidateAsync(contact, default)).ReturnsAsync(validationResult);
        await _contactApplication.AddAsync(contact);
        _mockContactRepository.Verify(repo => repo.AddAsync(contact), Times.Once);
    }

    [Theory]
    [InlineData("", "invalid-email", "123", "")]
    [InlineData("Valid Name", "invalid-email", "123", "11")]
    [InlineData("Valid Name", "valid@example.com", "123", "")]
    public async Task UpdateAsync_ShouldThrowValidationException_WhenContactIsInvalid(string name, string email, string phone, string ddd)
    {
        var contact = new Contact { Id = Guid.NewGuid(), Name = name, Email = email, Phone = phone, Ddd = ddd };
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required"),
            new ValidationFailure("Email", "Email is invalid"),
            new ValidationFailure("Phone", "Phone is invalid"),
            new ValidationFailure("Ddd", "Ddd is invalid")
        });
        _mockContactValidator.Setup(validator => validator.ValidateAsync(contact, default)).ReturnsAsync(validationResult);
        await Assert.ThrowsAsync<ValidationException>(() => _contactApplication.UpdateAsync(contact));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenContactDoesNotExist()
    {
        var contact = new Contact { Id = Guid.NewGuid(), Name = "Valid Name", Email = "valid@example.com", Phone = "12345678", Ddd = "11" };
        var validationResult = new ValidationResult();
        _mockContactValidator.Setup(validator => validator.ValidateAsync(contact, default)).ReturnsAsync(validationResult);
        _mockContactRepository.Setup(repo => repo.CheckIfExistsAsync(contact.Id)).ReturnsAsync(false);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _contactApplication.UpdateAsync(contact));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateContact_WhenContactIsValidAndExists()
    {
        var contact = new Contact { Id = Guid.NewGuid(), Name = "Valid Name", Email = "valid@example.com", Phone = "12345678", Ddd = "11" };
        var validationResult = new ValidationResult();
        _mockContactValidator.Setup(validator => validator.ValidateAsync(contact, default)).ReturnsAsync(validationResult);
        _mockContactRepository.Setup(repo => repo.CheckIfExistsAsync(contact.Id)).ReturnsAsync(true);
        await _contactApplication.UpdateAsync(contact);
        _mockContactRepository.Verify(repo => repo.UpdateAsync(contact), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenContactDoesNotExist()
    {
        var contactId = Guid.NewGuid();
        _mockContactRepository.Setup(repo => repo.GetAsync(contactId)).ReturnsAsync((Contact)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _contactApplication.DeleteAsync(contactId));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteContact_WhenContactExists()
    {
        var contactId = Guid.NewGuid();
        var contact = new Contact { Id = contactId, Name = "Test", Email = "test@example.com", Phone = "12345678", Ddd = "11" };
        _mockContactRepository.Setup(repo => repo.GetAsync(contactId)).ReturnsAsync(contact);
        await _contactApplication.DeleteAsync(contactId);
        _mockContactRepository.Verify(repo => repo.DeleteAsync(contact), Times.Once);
    }
}
