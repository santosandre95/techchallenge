using Moq;
using Application.Applications.Interfaces;
using Infrastructure.Repositories.Interface;
using Application.Applications;
using FluentValidation;
using Core.Entities;
using FluentValidation.Results;

namespace TechChallengeTest
{
    public class ContactApplicationTests
    {
        private readonly Mock<IContactRepository> _mockRepository;
        private readonly Mock<IValidator<Contact>> _mockValidator;
        private readonly IContactApplication _contactApplication;

        public ContactApplicationTests()
        {
            _mockRepository = new Mock<IContactRepository>();
            _mockValidator = new Mock<IValidator<Contact>>();
            _contactApplication = new ContactApplication(_mockRepository.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddContact_WhenContactIsValid()
        {
            // Arrange
            var contact = new Contact
            {
                Name = "João Silva",
                Email = "joao.silva@example.com",
                Phone = "987654321",
                Ddd = "11"
            };

            _mockValidator.Setup(v => v.ValidateAsync(contact, default))
                .ReturnsAsync(new ValidationResult());

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Contact>())).Returns(Task.CompletedTask);

            // Act
            await _contactApplication.AddAsync(contact);

            // Assert
            _mockValidator.Verify(v => v.ValidateAsync(contact, default), Times.Once);
            _mockRepository.Verify(repo => repo.AddAsync(It.Is<Contact>(c => c == contact)), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenEmailIsInvalid()
        {
            // Arrange
            var contact = new Contact
            {
                Name = "João Silva",
                Email = "email_invalido",
                Phone = "987654321",
                Ddd = "11"
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure(nameof(Contact.Email), "Email inválido.")
        });

            _mockValidator.Setup(v => v.ValidateAsync(contact, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _contactApplication.AddAsync(contact));
            Assert.Contains(exception.Errors, e => e.PropertyName == nameof(Contact.Email) && e.ErrorMessage == "Email inválido.");

            _mockValidator.Verify(v => v.ValidateAsync(contact, default), Times.Once);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenPhoneIsInvalid()
        {
            // Arrange
            var contact = new Contact
            {
                Name = "João Silva",
                Email = "joao.silva@example.com",
                Phone = "12345",
                Ddd = "11"
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure(nameof(Contact.Phone), "Phone deve ter 8 ou 9 dígitos numéricos.")
        });

            _mockValidator.Setup(v => v.ValidateAsync(contact, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _contactApplication.AddAsync(contact));
            Assert.Contains(exception.Errors, e => e.PropertyName == nameof(Contact.Phone) && e.ErrorMessage == "Phone deve ter 8 ou 9 dígitos numéricos.");

            _mockValidator.Verify(v => v.ValidateAsync(contact, default), Times.Once);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenDddIsInvalid()
        {
            // Arrange
            var contact = new Contact
            {
                Name = "João Silva",
                Email = "joao.silva@example.com",
                Phone = "987654321",
                Ddd = "123"
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure(nameof(Contact.Ddd), "DDD deve ter 2 dígitos numéricos.")
        });

            _mockValidator.Setup(v => v.ValidateAsync(contact, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _contactApplication.AddAsync(contact));
            Assert.Contains(exception.Errors, e => e.PropertyName == nameof(Contact.Ddd) && e.ErrorMessage == "DDD deve ter 2 dígitos numéricos.");

            _mockValidator.Verify(v => v.ValidateAsync(contact, default), Times.Once);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Never);
        }
    }
}


