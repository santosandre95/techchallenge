using Application.Applications.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace TechChallengeBuscaId.Controllers
{

    [ApiController]
    [Route("/buscaPorId")]
    public class BuscaPorIdController : ControllerBase
    {
        private readonly IContactApplication _contactApplication;
   


        public BuscaPorIdController(IContactApplication contactApplication)
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

    }
}
