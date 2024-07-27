using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories;
using Core.Entities;

public class ContactRepositoryTests
{
    private readonly ContactRepository _contactRepository;
    private readonly ApplicationDbContext _dbContext;

    public ContactRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _contactRepository = new ContactRepository(_dbContext);
    }

    private void Seed(params Contact[] contacts)
    {
        _dbContext.Contact.AddRange(contacts);
        _dbContext.SaveChanges();
    }

    private void ClearDatabase()
    {
        _dbContext.Contact.RemoveRange(_dbContext.Contact);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetContactsByDddAsync_ShouldReturnContacts_WhenContactsExist()
    {
        // Arrange
        ClearDatabase();
        Seed(
            new Contact { Id = Guid.NewGuid(), Name = "Test1", Email = "test1@example.com", Phone = "12345678", Ddd = "11" },
            new Contact { Id = Guid.NewGuid(), Name = "Test2", Email = "test2@example.com", Phone = "87654321", Ddd = "11" }
        );

        // Act
        var result = await _contactRepository.GetContactsByDddAsync("11");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetContactsByDddAsync_ShouldReturnEmptyList_WhenNoContactsExist()
    {
        // Arrange
        ClearDatabase();

        // Act
        var result = await _contactRepository.GetContactsByDddAsync("11");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddContact()
    {
        // Arrange
        ClearDatabase();
        var contact = new Contact { Id = Guid.NewGuid(), Name = "Test", Email = "test@example.com", Phone = "12345678", Ddd = "11" };

        // Act
        await _contactRepository.AddAsync(contact);

        // Assert
        var result = await _dbContext.Contact.FindAsync(contact.Id);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteContact()
    {
        // Arrange
        ClearDatabase();
        var contact = new Contact { Id = Guid.NewGuid(), Name = "Test", Email = "test@example.com", Phone = "12345678", Ddd = "11" };
        Seed(contact);

        // Act
        await _contactRepository.DeleteAsync(contact);

        // Assert
        var result = await _dbContext.Contact.FindAsync(contact.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnContact_WhenContactExists()
    {
        // Arrange
        ClearDatabase();
        var contactId = Guid.NewGuid();
        var contact = new Contact { Id = contactId, Name = "Test", Email = "test@example.com", Phone = "12345678", Ddd = "11" };
        Seed(contact);

        // Act
        var result = await _contactRepository.GetAsync(contactId);

        // Assert
        Assert.Equal(contact, result);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenContactDoesNotExist()
    {
        // Arrange
        ClearDatabase();
        var contactId = Guid.NewGuid();

        // Act
        var result = await _contactRepository.GetAsync(contactId);

        // Assert
        Assert.Null(result);
    }
}
