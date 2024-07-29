using Application.Applications.Interfaces;
using Core.Entities;
using Core.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace TechChallengeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactApplication _contactApplication;

        public ContactController(IContactApplication contactApplication)
        {
            _contactApplication = contactApplication;
        }

        /// <summary>
        /// Busca contato por Id.
        /// </summary>
        /// <param name="id">Id do contato.</param>
        /// <returns>Um contato.</returns>
        /// <response code="200">Contato retornado com sucesso!</response>
        /// <response code="404">Contato n�o encontrado :(</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> Get(Guid id)
        {
            try
            {
                var contact = await _contactApplication.GetAsync(id);
                return Ok(contact);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Cria um novo contato.
        /// </summary>
        /// <param name="contact">Informa��es do contato a ser criado.</param>
        /// <returns>Contato criado.</returns>
        /// <response code="201">Contato criado com sucesso!</response>
        /// <response code="400">Conte�do inv�lido :(</response>
        [HttpPost]
        public async Task<ActionResult<Contact>> Add(Contact contact)
        {
            try
            {
                await _contactApplication.AddAsync(contact);
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
        /// <response code="400">Conte�do inv�lido T.T</response>
        /// <response code="404">Contato n�o encontrado :(</response>
        [HttpPut]
        public async Task<IActionResult> Update(Contact contact)
        {
            try
            {
                await _contactApplication.UpdateAsync(contact);
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
                await _contactApplication.DeleteAsync(id);
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
            var contacts = await _contactApplication.GetAllAsync();
            return Ok(contacts);
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
            var contacts = await _contactApplication.GetContactsByDddAsync(ddd);
            return Ok(contacts);
        }
    }
}
