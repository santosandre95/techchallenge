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
using TechChallengeApi.Events;
using TechChallengeApi.RabbitMq;

public class ContactControllerTests
{
    private readonly Mock<IContactApplication> _mockContactApplication;
    private readonly Mock<RabbitMqEventBus> _mockEventBus;
    private readonly ContactController _contactController;

    public ContactControllerTests()
    {
        _mockContactApplication = new Mock<IContactApplication>();
        _mockEventBus = new Mock<RabbitMqEventBus>();
        _contactController = new ContactController(_mockContactApplication.Object, _mockEventBus.Object);
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

        _mockEventBus.Setup(bus => bus.PublishBuscaId(It.IsAny<BuscaIdEvent>()));
        _mockContactApplication.Setup(app => app.GetAsync(contactId)).ReturnsAsync(contact);

        var result = await _contactController.Get(contactId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        _mockEventBus.Verify(bus => bus.PublishBuscaId(It.IsAny<BuscaIdEvent>()), Times.Once);
        Assert.Equal(contactId, ((BuscaIdEvent)okResult.Value).Id);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenContactDoesNotExist()
    {
        var contactId = Guid.NewGuid();
        _mockEventBus.Setup(bus => bus.PublishBuscaId(It.IsAny<BuscaIdEvent>()));
        _mockContactApplication.Setup(app => app.GetAsync(contactId)).ThrowsAsync(new KeyNotFoundException());

        var result = await _contactController.Get(contactId);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Add_ShouldReturnCreatedAtAction_WhenContactIsValid()
    {
        var contact = CreateContact();
        _mockEventBus.Setup(bus => bus.PublishCreated(It.IsAny<CreateEvent>()));
        _mockContactApplication.Setup(app => app.AddAsync(contact)).Returns(Task.CompletedTask);

        var result = await _contactController.Add(contact);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        _mockEventBus.Verify(bus => bus.PublishCreated(It.IsAny<CreateEvent>()), Times.Once);
        Assert.Equal(nameof(_contactController.Get), createdAtActionResult.ActionName);
        Assert.Equal(contact, createdAtActionResult.Value);
    }

    //[Theory]
    //[InlineData("", "invalid-email", "123", "")]
    //[InlineData("Valid Name", "invalid-email", "123", "11")]
    //public async Task Add_ShouldReturnBadRequest_WhenContactIsInvalid(string name, string email, string phone, string ddd)
    //{
    //    var contact = CreateContact(name, email, phone, ddd);
    //    var validationFailures = new List<ValidationFailure>
    //    {
    //        new ValidationFailure("Name", "Name is required"),
    //        new ValidationFailure("Email", "Email is invalid"),
    //        new ValidationFailure("Phone", "Phone is invalid"),
    //        new ValidationFailure("Ddd", "Ddd is invalid")
    //    };
    //    var validationException = new ValidationException(validationFailures);

    //    _mockEventBus.Setup(bus => bus.PublishCreated(It.IsAny<CreateEvent>()));
    //    _mockContactApplication.Setup(app => app.AddAsync(contact)).ThrowsAsync(validationException);

    //    var result = await _contactController.Add(contact);

    //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
    //    var errors = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(badRequestResult.Value.GetType().GetProperty("errors").GetValue(badRequestResult.Value, null));
    //    Assert.Equal(validationFailures, errors);
    //}

    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenContactIsValidAndExists()
    {
        var contact = CreateContact();
        _mockEventBus.Setup(bus => bus.PublishUpdated(It.IsAny<UpdateEvent>()));
        _mockContactApplication.Setup(app => app.UpdateAsync(contact)).Returns(Task.CompletedTask);

        var result = await _contactController.Update(contact);

        Assert.IsType<NoContentResult>(result);
        _mockEventBus.Verify(bus => bus.PublishUpdated(It.IsAny<UpdateEvent>()), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenContactDoesNotExist()
    {
        var contact = CreateContact();
        _mockEventBus.Setup(bus => bus.PublishUpdated(It.IsAny<UpdateEvent>()));
        _mockContactApplication.Setup(app => app.UpdateAsync(contact)).ThrowsAsync(new KeyNotFoundException());

        var result = await _contactController.Update(contact);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenContactExists()
    {
        var contactId = Guid.NewGuid();
        _mockEventBus.Setup(bus => bus.PublishDeleted(It.IsAny<DeleteEvent>()));
        _mockContactApplication.Setup(app => app.DeleteAsync(contactId)).Returns(Task.CompletedTask);

        var result = await _contactController.Delete(contactId);

        Assert.IsType<NoContentResult>(result);
        _mockEventBus.Verify(bus => bus.PublishDeleted(It.IsAny<DeleteEvent>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenContactDoesNotExist()
    {
        var contactId = Guid.NewGuid();
        _mockEventBus.Setup(bus => bus.PublishDeleted(It.IsAny<DeleteEvent>()));
        _mockContactApplication.Setup(app => app.DeleteAsync(contactId)).ThrowsAsync(new KeyNotFoundException());

        var result = await _contactController.Delete(contactId);

        Assert.IsType<NotFoundResult>(result);
    }
}