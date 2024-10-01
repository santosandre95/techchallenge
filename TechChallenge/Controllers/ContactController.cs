using Application.Applications.Interfaces;
using Core.Entities;
using Core.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TechChallengeApi.Events;
using TechChallengeApi.RabbitMq;

namespace TechChallengeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactApplication _contactApplication;
        private readonly RabbitMqEventBus _eventBus;

        public ContactController(IContactApplication contactApplication, RabbitMqEventBus eventBus)
        {
            _contactApplication = contactApplication;
            _eventBus = eventBus;
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
                var contactBuscaEvent = new BuscaIdEvent(id);
                await _eventBus.PublishBuscaId(contactBuscaEvent);
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
        /// <response code="400">Conte�do inválido :(</response>
        [HttpPost]
        public async Task<ActionResult<Contact>> Add(Contact contact)
        {
            try
            {

                var contactCreate = new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = contact.Name,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    Ddd = contact.Ddd
                };


                CreateEvent createEvent = new CreateEvent
                                            (
                                            contactCreate.Id,
                                            contactCreate.Name,
                                            contactCreate.Email, 
                                            contactCreate.Phone, 
                                            contactCreate.Ddd
                                            );

                await _eventBus.PublishCreated(createEvent);

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
        /// <response code="400">Conte�do inválido T.T</response>
        /// <response code="404">Contato n�o encontrado :(</response>
        [HttpPut]
        public async Task<IActionResult> Update(Contact contact)
        {
            try
            {
                var contactUpdateEvent = new UpdateEvent(contact);
                await _eventBus.PublishUpdated(contactUpdateEvent);
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
        /// <returns>Sem conte�do.</returns>
        /// <response code="204">Contato exclu�do com sucesso!</response>
        /// <response code="404">Contato n�o encontrado T.T</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var contactDeleteEvent = new DeleteEvent(id);
                await _eventBus.PublishDeleted(contactDeleteEvent);
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
            var contactBuscaTodosEvent = new BuscaTodosEvent();
            await _eventBus.PublishBuscaTodos(contactBuscaTodosEvent);
            return Ok(contactBuscaTodosEvent);
        }

        /// <summary>
        /// Busca todos os contatos com base em um DDD.
        /// </summary>
        /// <param name="ddd">DDD da regi�o do telefone.</param>
        /// <returns>Retorna uma lista de contatos com o DDD informado.</returns>
        /// <response code="200">Lista de contatos por DDD filtrado com sucesso!</response>
        [HttpGet("Ddd/{ddd}")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContactsByDdd(string ddd)
        {
            var contactBuscaDddEvent = new BuscaDddEvent(ddd);
           await _eventBus.PublishBuscaDdd(contactBuscaDddEvent);
            return Ok(contactBuscaDddEvent);
        }
    }
}
