using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Application.Applications.Interfaces;
using Core.Entities;
using TechChallengeApi.Controllers;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ContactControllerTests
{
    private readonly Mock<IContactApplication> _mockContactApplication;
    private readonly ContactController _contactController;

    public ContactControllerTests()
    {
        _mockContactApplication = new Mock<IContactApplication>();
        _contactController = new ContactController(_mockContactApplication.Object);
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
    public async Task Get_ShouldReturnOk_WhenContactExists()
    {
        var contactId = Guid.NewGuid();
        var contact = CreateContact();
        contact.Id = contactId;
        _mockContactApplication.Setup(app => app.GetAsync(contactId)).ReturnsAsync(contact);
        var result = await _contactController.Get(contactId);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(contact, okResult.Value);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenContactDoesNotExist()
    {
        var contactId = Guid.NewGuid();
        _mockContactApplication.Setup(app => app.GetAsync(contactId)).ThrowsAsync(new KeyNotFoundException());
        var result = await _contactController.Get(contactId);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Add_ShouldReturnCreatedAtAction_WhenContactIsValid()
    {
        var contact = CreateContact();
        _mockContactApplication.Setup(app => app.AddAsync(contact)).Returns(Task.CompletedTask);
        var result = await _contactController.Add(contact);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(_contactController.Get), createdAtActionResult.ActionName);
        Assert.Equal(contact, createdAtActionResult.Value);
    }

    [Theory]
    [InlineData("", "invalid-email", "123", "")]
    [InlineData("Valid Name", "invalid-email", "123", "11")]
    public async Task Add_ShouldReturnBadRequest_WhenContactIsInvalid(string name, string email, string phone, string ddd)
    {
        var contact = CreateContact(name, email, phone, ddd);
        var validationFailures = new List<ValidationFailure> 
        { 
            new ValidationFailure("Name", "Name is required"),
            new ValidationFailure("Email", "Email is invalid"),
            new ValidationFailure("Phone", "Phone is invalid"),
            new ValidationFailure("Ddd", "Ddd is invalid")
        };
        var validationException = new ValidationException(validationFailures);
        _mockContactApplication.Setup(app => app.AddAsync(contact)).ThrowsAsync(validationException);
        var result = await _contactController.Add(contact);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var errors = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(badRequestResult.Value.GetType().GetProperty("errors").GetValue(badRequestResult.Value, null));
        Assert.Equal(validationFailures, errors);
    }

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenContactIsValidAndExists()
    {
        var contact = CreateContact();
        _mockContactApplication.Setup(app => app.UpdateAsync(contact)).Returns(Task.CompletedTask);
        var result = await _contactController.Update(contact);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenContactDoesNotExist()
    {
        var contact = CreateContact();
        _mockContactApplication.Setup(app => app.UpdateAsync(contact)).ThrowsAsync(new KeyNotFoundException());
        var result = await _contactController.Update(contact);
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData("", "invalid-email", "123", "")]
    [InlineData("Valid Name", "invalid-email", "123", "11")]
    public async Task Update_ShouldReturnBadRequest_WhenContactIsInvalid(string name, string email, string phone, string ddd)
    {
        var contact = CreateContact(name, email, phone, ddd);
        var validationFailures = new List<ValidationFailure> 
        { 
            new ValidationFailure("Name", "Name is required"),
            new ValidationFailure("Email", "Email is invalid"),
            new ValidationFailure("Phone", "Phone is invalid"),
            new ValidationFailure("Ddd", "Ddd is invalid")
        };
        var validationException = new ValidationException(validationFailures);
        _mockContactApplication.Setup(app => app.UpdateAsync(contact)).ThrowsAsync(validationException);
        var result = await _contactController.Update(contact);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errors = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(badRequestResult.Value.GetType().GetProperty("errors").GetValue(badRequestResult.Value, null));
        Assert.Equal(validationFailures, errors);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenContactExists()
    {
        var contactId = Guid.NewGuid();
        _mockContactApplication.Setup(app => app.DeleteAsync(contactId)).Returns(Task.CompletedTask);
        var result = await _contactController.Delete(contactId);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenContactDoesNotExist()
    {
        var contactId = Guid.NewGuid();
        _mockContactApplication.Setup(app => app.DeleteAsync(contactId)).ThrowsAsync(new KeyNotFoundException());
        var result = await _contactController.Delete(contactId);
        Assert.IsType<NotFoundResult>(result);
    }
}
