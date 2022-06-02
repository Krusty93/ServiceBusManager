using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ServiceBusManager.Server.Application.Queries;

namespace ServiceBusManager.Server.API.Controllers
{
    [ApiController]
    [Route("api/queue/")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class QueueController : ControllerBase
    {
        private readonly IServiceBusQueries _serviceBusQueries;

        public QueueController(IServiceBusQueries serviceBusQueries)
        {
            _serviceBusQueries = serviceBusQueries;
        }

        /// <summary>
        /// Get queue list
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET: /api/queue
        /// 
        /// </remarks>
        /// <response code="200">Returns list of available queues</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(QueueGetAllResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetQueuesAsync()
        {
            var queues = await _serviceBusQueries.GetAllQueuesAsync();
            return Ok(queues);
        }

        /// <summary>
        /// Get queue details
        /// </summary>
        /// <remarks>
        /// 
        ///     GET: /api/queue/{name}
        /// 
        /// </remarks>
        /// <response code="200">Returns queue details</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [HttpGet]
        [Route("{name}")]
        [ProducesResponseType(typeof(QueueGetDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetQueueDetailsAsync([FromRoute] string name)
        {
            var details = await _serviceBusQueries.GetQueueDetailsAsync(name);
            return Ok(details);
        }
    }
}
