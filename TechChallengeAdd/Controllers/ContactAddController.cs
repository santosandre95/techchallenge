using Application.Applications.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TechChallengeAdd.Controllers
{
    [Route("addService/[controller]")]
    [ApiController]
    public class ContactAddController :ControllerBase
    {

        private readonly IContactApplication _contactApplication;

        public ContactAddController(IContactApplication contactApplication)
        {
            _contactApplication = contactApplication;
        }
    
  
    }

}
