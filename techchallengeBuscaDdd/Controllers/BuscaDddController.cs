using Application.Applications.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace techchallengeBuscaDdd.Controllers
{
    [Route("buscadd_service/[controller]")]
    [ApiController]
    public class BuscaDddController : ControllerBase
    {
        private readonly IContactApplication _contactApplication;
        public BuscaDddController(IContactApplication contactApplication)
        {
            _contactApplication = contactApplication;
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
