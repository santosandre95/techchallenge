using Moq;
using Core.Entities;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Repositories.Interface;
using Application.Applications;
using Microsoft.AspNetCore.Mvc;
using TechChallengeApi.Controllers;
using TechChallengeApi.RabbitMqClient;

public class ContactApplicationTests
{
    private readonly Mock<IRabbitMqClient> _mockRabbitMqClient;
    private readonly ContactController _controller;
    private readonly Contact contact;

    public ContactApplicationTests()
    {
        _mockRabbitMqClient = new Mock<IRabbitMqClient>();
        _controller = new ContactController(_mockRabbitMqClient.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WhenContactExists()
    {
        var contactId = Guid.NewGuid();
        _mockRabbitMqClient.Setup(client => client.BuscaPoID(contactId));

        var result = await _controller.Get(contactId);

        var actionResult = Assert.IsType<ActionResult<Contact>>(result);
        Assert.IsType<OkResult>(actionResult.Result);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenContactDoesNotExist()
    {
        var contactId = Guid.NewGuid();
        _mockRabbitMqClient.Setup(client => client.BuscaPoID(contactId)).Throws(new KeyNotFoundException());

        var result = await _controller.Get(contactId);

        var actionResult = Assert.IsType<ActionResult<Contact>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task Add_ShouldReturnOk_WhenContactIsValid()
    {
        var contact = new Contact { Name = "Test", Email = "test@example.com", Phone = "12345678", Ddd = "11" };
        _mockRabbitMqClient.Setup(client => client.AddContato(contact));
          

        var result = await _controller.Add(contact);

        var actionResult = Assert.IsType<ActionResult<Contact>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(contact, okResult.Value);
        
    }



    [Fact]
    public async Task Update_ShouldReturnNoContent_WhenContactIsUpdated()
    {
        var contact = new Contact { Id = Guid.NewGuid(), Name = "Updated", Email = "updated@example.com", Phone = "87654321", Ddd = "22" };
        _mockRabbitMqClient.Setup(client => client.AtualizaContato(contact));

        var result = await _controller.Update(contact);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenContactDoesNotExist()
    {
        var contact = new Contact { Id = Guid.NewGuid(), Name = "Nonexistent" , Email = "updated@example.com", Phone = "87654321", Ddd = "22" };
        _mockRabbitMqClient.Setup(client => client.AtualizaContato(contact)).Throws(new KeyNotFoundException());

        var result = await _controller.Update(contact);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenContactIsDeleted()
    {
        var contactId = Guid.NewGuid();
        _mockRabbitMqClient.Setup(client => client.RemoveContato(contactId));

        var result = await _controller.Delete(contactId);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenContactDoesNotExist()
    {
        var contactId = Guid.NewGuid();
        _mockRabbitMqClient.Setup(client => client.RemoveContato(contactId)).Throws(new KeyNotFoundException());

        var result = await _controller.Delete(contactId);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WhenContactsAreFetched()
    {
        _mockRabbitMqClient.Setup(client => client.Buscatodos());

        var result = await _controller.GetAll();

        Assert.IsType<OkResult>(result.Result);
    }

    [Fact]
    public async Task GetContactsByDdd_ShouldReturnOk_WhenContactsAreFetchedByDdd()
    {
        var ddd = "11";
        _mockRabbitMqClient.Setup(client => client.BuscaPorDdd(ddd));

        var result = await _controller.GetContactsByDdd(ddd);

        Assert.IsType<OkResult>(result.Result);
    }
}
