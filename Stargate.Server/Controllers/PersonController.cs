using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Server.Business.Commands;
using Stargate.Server.Business.Queries;
using System.Net;
using System.Text.Json;

namespace Stargate.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PersonController> _logger;
        public PersonController(IMediator mediator, ILogger<PersonController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all people recorded in system
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetPeople(CancellationToken cancellationToken)
        {           
            var result = await _mediator.Send(new GetPeople()
            {

            }, cancellationToken);
            _logger.LogInformation(JsonSerializer.Serialize(result));
            return this.GetResponse(result);                        
        }

        /// <summary>
        /// Retrieves a person by name
        /// </summary>
        /// <param name="name">Full name of existing person</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetPersonByName(string name, CancellationToken cancellationToken)
        {            
            var result = await _mediator.Send(new GetPersonByName()
            {
                Name = name
            }, cancellationToken);
            _logger.LogInformation(JsonSerializer.Serialize(result));
            return this.GetResponse(result);            
        }

        /// <summary>
        /// Creates a new person in the system
        /// </summary>
        /// <param name="name">Full name of new person</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> CreatePerson([FromBody] string name, CancellationToken cancellationToken)
        {            
            var result = await _mediator.Send(new CreatePerson()
            {
                Name = name
            }, cancellationToken);
            _logger.LogInformation(JsonSerializer.Serialize(result));
            return this.GetResponse(result);            
        }

        /// <summary>
        /// Updates Person by Full Name
        /// </summary>
        /// <param name="name">Current Full Name, passed in URI</param>
        /// <param name="newName">New Full Name, passed in body</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdatePersonByName(string name, [FromBody] string newName, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdatePerson()
            {
                Name = name,
                NewName = newName
            }, cancellationToken);
            _logger.LogInformation(JsonSerializer.Serialize(result));
            return this.GetResponse(result);            
        }
    }
}
