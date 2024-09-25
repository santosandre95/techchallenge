using Application.Applications.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace TechChallengeBuscaTodos.Controllers
{
    [ApiController]
    [Route("/GetAll")]
    public class BuscaController : ControllerBase
    {
        private readonly IContactApplication _contactApplication;

        public BuscaController(IContactApplication contactApplication)
        {
            _contactApplication = contactApplication;
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
    }
}
