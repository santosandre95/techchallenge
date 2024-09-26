using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using TechChallengeBuscaTodos.RabbitMqClient;

namespace TechChallengeBuscaTodos.Controllers
{
   
    public class BucaTodosController : Controller
    {
        /// <summary>
        /// Busca todos os contatos.
        /// </summary>
        /// <returns>Uma lista de contatos.</returns>
        /// <response code="200">Lista de contatos retornada com sucesso!</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAll()
        {

         
            //var contacts = await _rabbitMqConsumer.GetContactsAsync(); 
            return Ok();
        }

    }
}
