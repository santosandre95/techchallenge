using Application.Applications.Interfaces;
using Core.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace TechChallengeAdd.Controllers
{

    [ApiController]
    [Route("/Add")]
    public class AddController: ControllerBase
    {
        private readonly IContactApplication _contactApplication;
 
        public AddController(IContactApplication contactApplication)
        {
            _contactApplication = contactApplication;
      
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
                return CreatedAtAction(nameof(Guid), new { id = contact.Id }, contact);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors });
            }
        }

    }
}
