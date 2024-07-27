using Core.Entities;
using Core.Validations;
using FluentValidation.TestHelper;

public class ContactValidatorTests
{
    private readonly ContactValidator _validator;

    public ContactValidatorTests()
    {
        _validator = new ContactValidator();
    }

    private Contact CreateContact(string name = "Test", string email = "test@example.com", string phone = "12345678", string ddd = "11")
    {
        return new Contact
        {
            Name = name,
            Email = email,
            Phone = phone,
            Ddd = ddd
        };
    }

    [Fact]
    public void ShouldHaveError_WhenEmailIsEmpty()
    {
        var contact = CreateContact(email: string.Empty);
        var result = _validator.TestValidate(contact);
        result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("O email é obrigatório.");
    }

    [Fact]
    public void ShouldHaveError_WhenEmailIsInvalid()
    {
        var contact = CreateContact(email: "invalid-email");
        var result = _validator.TestValidate(contact);
        result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("O email deve ser válido.");
    }

    [Fact]
    public void ShouldNotHaveError_WhenEmailIsValid()
    {
        var contact = CreateContact(email: "test@example.com");
        var result = _validator.TestValidate(contact);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void ShouldHaveError_WhenDddIsEmpty()
    {
        var contact = CreateContact(ddd: string.Empty);
        var result = _validator.TestValidate(contact);
        result.ShouldHaveValidationErrorFor(c => c.Ddd).WithErrorMessage("O DDD é obrigatório.");
    }

    [Theory]
    [InlineData("1")]
    [InlineData("123")]
    public void ShouldHaveError_WhenDddIsInvalid(string ddd)
    {
        var contact = CreateContact(ddd: ddd);
        var result = _validator.TestValidate(contact);
        result.ShouldHaveValidationErrorFor(c => c.Ddd).WithErrorMessage("O DDD deve ter dois dígitos numéricos.");
    }

    [Fact]
    public void ShouldNotHaveError_WhenDddIsValid()
    {
        var contact = CreateContact(ddd: "11");
        var result = _validator.TestValidate(contact);
        result.ShouldNotHaveValidationErrorFor(c => c.Ddd);
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        var contact = CreateContact(name: string.Empty);
        var result = _validator.TestValidate(contact);
        result.ShouldHaveValidationErrorFor(c => c.Name).WithErrorMessage("O Nome é obrigatório.");
    }

    [Fact]
    public void ShouldNotHaveError_WhenNameIsValid()
    {
        var contact = CreateContact(name: "Test Name");
        var result = _validator.TestValidate(contact);
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void ShouldHaveError_WhenPhoneIsEmpty()
    {
        var contact = CreateContact(phone: string.Empty);
        var result = _validator.TestValidate(contact);
        result.ShouldHaveValidationErrorFor(c => c.Phone).WithErrorMessage("O telefone é obrigatório.");
    }

    [Theory]
    [InlineData("123")]
    [InlineData("1234567890")]
    public void ShouldHaveError_WhenPhoneIsInvalid(string phone)
    {
        var contact = CreateContact(phone: phone);
        var result = _validator.TestValidate(contact);
        result.ShouldHaveValidationErrorFor(c => c.Phone).WithErrorMessage("Informe somente números no telefone sem DDD");
    }

    [Fact]
    public void ShouldNotHaveError_WhenPhoneIsValid()
    {
        var contact = CreateContact(phone: "12345678");
        var result = _validator.TestValidate(contact);
        result.ShouldNotHaveValidationErrorFor(c => c.Phone);
    }
}
