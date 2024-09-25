using Application.Applications.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace techchallengeUpdate.Controllers
{
    [ApiController]
    [Route("/Update")]
    public class UpdateController : ControllerBase
    {
        private readonly IContactApplication _contactApplication;
        public UpdateController(IContactApplication contactApplication)
        {
            _contactApplication = contactApplication;
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
                return BadRequest(new { errors = ex.ValidationResult });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
