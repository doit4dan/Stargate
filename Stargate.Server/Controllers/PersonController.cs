using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Server.Business.Commands;
using Stargate.Server.Business.Queries;
using System.Net;

namespace Stargate.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all people recorded in system
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetPeople(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(new GetPeople()
                {

                }, cancellationToken);

                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
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
            try
            {
                var result = await _mediator.Send(new GetPersonByName()
                {
                    Name = name
                }, cancellationToken);

                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
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
            try
            {
                var result = await _mediator.Send(new CreatePerson()
                {
                    Name = name
                }, cancellationToken);

                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
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
            try
            {
                var result = await _mediator.Send(new UpdatePerson()
                {
                    Name = name,
                    NewName = newName
                }, cancellationToken);

                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                return this.GetResponse(new BaseResponse()
                {
                    Message = ex.Message,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }
    }
}
