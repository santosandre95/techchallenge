using Application.Applications.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace techchallengeDelete.Controllers
{
    [ApiController]
    [Route("/Delete")]
    public class DeleteController : ControllerBase
    {
        private readonly IContactApplication _contactApplication;
        public DeleteController(IContactApplication contactApplication)
        {
            _contactApplication = contactApplication;
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

    }
}
