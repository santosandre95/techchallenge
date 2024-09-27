using Application.Applications.Interfaces;
using Core.Entities;
using Core.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TechChallengeApi.Events;
using TechChallengeApi.Events.TechChallengeApi.Events;
using TechChallengeApi.RabbitMqEvents;

namespace TechChallengeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactApplication _contactApplication;
        private readonly RabbitMqEventBus _eventBus;
        private IContactApplication @object;

        public ContactController(IContactApplication contactApplication, RabbitMqEventBus eventBus)
        {
            _contactApplication = contactApplication;
            _eventBus = eventBus;
        }

        public ContactController(IContactApplication @object)
        {
            this.@object = @object;
        }

        /// <summary>
        /// Busca contato por Id.
        /// </summary>
        /// <param name="id">Id do contato.</param>
        /// <returns>Um contato.</returns>
        /// <response code="200">Contato retornado com sucesso!</response>
        /// <response code="404">Contato não encontrado :(</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> Get(Guid id)
        {
            try
            {
                var contactBuscaEvent = new ContactBuscaIdEvent(id);
                _eventBus.PublishContactBuscaId(contactBuscaEvent);
                return Ok(contactBuscaEvent);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Cria um novo contato.
        /// </summary>
        /// <param name="contact">Informações do contato a ser criado.</param>
        /// <returns>Contato criado.</returns>
        /// <response code="201">Contato criado com sucesso!</response>
        /// <response code="400">Conteúdo inválido :(</response>
        [HttpPost]
        public async Task<ActionResult<Contact>> Add(Contact contact)
        {
            try
            {
                var contactCreatedEvent = new ContactCreatedEvent(contact);
                _eventBus.PublishContactCreated(contactCreatedEvent);

                return CreatedAtAction(nameof(Get), new { id = contact.Id }, contact);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
        }

        /// <summary>
        /// Atualiza um contato.
        /// </summary>
        /// <param name="contact">Objeto de contato atualizado.</param>
        /// <returns>Retorna status code.</returns>
        /// <response code="204">Contato atualizado com sucesso!</response>
        /// <response code="400">Conteúdo inválido T.T</response>
        /// <response code="404">Contato não encontrado :(</response>
        [HttpPut]
        public async Task<IActionResult> Update(Contact contact)
        {
            try
            {
                var contactUpdateEvent = new ContactUpdateEvent(contact);
                _eventBus.PublishContactUpdated(contactUpdateEvent);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Exclui contato por Id.
        /// </summary>
        /// <param name="id">Id do contato.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Contato excluído com sucesso!</response>
        /// <response code="404">Contato não encontrado T.T</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var contactDeleteEvent = new ContactDeletedEvent(id);
                _eventBus.PublishContactDeleted(contactDeleteEvent); 
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Busca todos os contatos.
        /// </summary>
        /// <returns>Uma lista de contatos.</returns>
        /// <response code="200">Lista de contatos retornada com sucesso!</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAll()
        {
            var contactBuscaTodosEvent = new ContactBuscaTodosEvent();
            _eventBus.PublishContactBuscaTodos(contactBuscaTodosEvent);
            return Ok(contactBuscaTodosEvent);
        }

        /// <summary>
        /// Busca todos os contatos com base em um DDD.
        /// </summary>
        /// <param name="ddd">DDD da região do telefone.</param>
        /// <returns>Retorna uma lista de contatos com o DDD informado.</returns>
        /// <response code="200">Lista de contatos por DDD filtrado com sucesso!</response>
        [HttpGet("Ddd/{ddd}")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContactsByDdd(string ddd)
        {
            var contactBuscaDddEvent = new ContactBuscaDddEvent(ddd);
            _eventBus.PublishContactBuscaDdd(contactBuscaDddEvent);
            return Ok(contactBuscaDddEvent);
        }
    }
}
