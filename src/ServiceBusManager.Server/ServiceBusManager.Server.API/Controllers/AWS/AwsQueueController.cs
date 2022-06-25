using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ServiceBusManager.Server.API.Filters;

namespace ServiceBusManager.Server.API.Controllers.AWS
{
    [ApiController]
    [Route("api/aws/queue/")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [AwsProviderTypeFilter]
    [ApiExplorerSettings(GroupName = SwaggerDocumentation.AWS_GROUP)]
    public class AwsQueueController : ControllerBase
    {
    }
}
