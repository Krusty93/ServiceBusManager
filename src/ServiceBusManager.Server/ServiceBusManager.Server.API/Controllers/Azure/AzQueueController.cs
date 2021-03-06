using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceBusManager.Server.API.Filters;
using ServiceBusManager.Server.Application.Commands;
using ServiceBusManager.Server.Application.Commands.Azure;
using ServiceBusManager.Server.Application.Queries;

namespace ServiceBusManager.Server.API.Controllers.Azure
{
    [ApiController]
    [Route("api/az/queue/")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [AzureProviderTypeFilter]
    [ApiExplorerSettings(GroupName = SwaggerDocumentation.AZ_GROUP)]
    public class AzQueueController : ControllerBase
    {
        private readonly IServiceBusQueries _serviceBusQueries;
        private readonly IMediator _mediator;

        public AzQueueController(IServiceBusQueries serviceBusQueries, IMediator mediator)
        {
            _serviceBusQueries = serviceBusQueries;
            _mediator = mediator;
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
        /// Sample request:
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
        [ProducesResponseType(typeof(QueueGetAllMessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetQueueDetailsAsync([FromRoute] string name)
        {
            var details = await _serviceBusQueries.GetQueueDetailsAsync(name);
            return Ok(details);
        }

        [HttpGet]
        [Route("{name}/messages")]
        [ProducesResponseType(typeof(QueueGetAllMessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetMessagesAsync([FromRoute] string name)
        {
            var messages = await _serviceBusQueries.GetQueueActiveMessagesAsync(name);
            return Ok(messages);
        }

        /// <summary>
        /// Create a new queue
        /// </summary>
        /// <param name="name">Queue name</param>
        /// <param name="command">Body request</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST: /api/queue/{name}
        ///     {
        ///       "autoDeleteOnIdle": "00:05:00",
        ///       "defaultMessageTimeToLive": "00:01:00",
        ///       "lockDuration": "00:01:00",
        ///       "duplicateDetectionHistoryTimeWindow": "00:01:00",
        ///       "maxSizeInMegabytes": 1024,
        ///       "maxDeliveryCount": 1,
        ///       "enableBatchedOperations": false,
        ///       "enablePartitioning": false,
        ///       "requireSession": false,
        ///       "requireDuplicateDetection": false
        ///     }
        /// 
        /// </remarks>
        /// <response code="201">Operation succeeded,</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [HttpPost]
        [Route("{name}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreateQueueAsync(
            [FromRoute] string name,
            [FromBody] CreateAzureQueueCommand command)
        {
            command.Name = name;

            await _mediator.Send(command);

            return Created(name, null);
        }

        /// <summary>
        /// Delete queue
        /// </summary>
        /// <param name="name">Queue name</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE: /api/queue/{name}
        /// 
        /// </remarks>
        /// <response code="204">Operation succeeded</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [HttpDelete]
        [Route("{name}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> DeleteQueueAsync([FromRoute] string name)
        {
            var cmd = new DeleteQueueCommand(name);
            await _mediator.Send(cmd);

            return NoContent();
        }

        /// <summary>
        /// Purge active messages of the given queue
        /// </summary>
        /// <param name="name">Queue name</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE: api/queue/{name}/active
        /// 
        /// </remarks>
        /// <response code="204">Operation succeeded</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [HttpDelete]
        [Route("{name}/active")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> PurgeQueueAsync([FromRoute] string name)
        {
            var command = new PurgeActiveQueueCommand(name);

            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Purge dead letter messages of the given queue
        /// </summary>
        /// <param name="name">Queue name</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE: api/queue/{name}/dlq
        /// 
        /// </remarks>
        /// <response code="204">Operation succeeded</response>
        /// <response code="500">If the server can't process the request</response>
        /// <response code="503">If the server is not ready to handle the request</response>
        [HttpDelete]
        [Route("{name}/dlq")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> PurgeDeadLetterQueueAsync([FromRoute] string name)
        {
            var command = new PurgeDeadLetterQueueCommand(name);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}
