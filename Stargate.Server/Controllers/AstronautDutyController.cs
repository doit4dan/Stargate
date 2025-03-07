﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Server.Business.Commands;
using Stargate.Server.Business.Queries;
using System.Net;
using System.Text.Json;

namespace Stargate.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AstronautDutyController> _logger;
        public AstronautDutyController(IMediator mediator, ILogger<AstronautDutyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves Astronaut Duties by Person Name
        /// </summary>
        /// <param name="name">Full name of existing person in the system</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetAstronautDutiesByName(string name, CancellationToken cancellationToken)
        {            
            var result = await _mediator.Send(new GetAstronautDutiesByName()
            {
                Name = name
            }, cancellationToken);            
            _logger.LogInformation(JsonSerializer.Serialize(result));
            return this.GetResponse(result);            
        }

        /// <summary>
        /// Creates new Astronaut Duty for existing Person in system
        /// </summary>
        /// <param name="request">Details of new Astronaut Duty to add to system</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDuty request, CancellationToken cancellationToken)
        {            
            var result = await _mediator.Send(request, cancellationToken);
            _logger.LogInformation(JsonSerializer.Serialize(result));
            return this.GetResponse(result);            
        }                
    }
}
