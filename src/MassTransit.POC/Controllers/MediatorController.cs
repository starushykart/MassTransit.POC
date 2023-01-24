using MassTransit.Mediator;
using Mediator.Events;
using Microsoft.AspNetCore.Mvc;

namespace MassTransit.POC.Controllers;

[ApiController]
[Route("[controller]")]
public class MediatorController : ControllerBase
{
    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromServices]IMediator mediator, [FromBody]IdModel model)
    {
        var client = mediator.CreateRequestClient<IAcceptMediatorAction>();
        var response = await client.GetResponse<IMediatorActionAccepted>(new
        {
            model.Id
        });
        
        return Ok(response.Message.Text);
    }
    
    public class IdModel
    {
        public Guid? Id { get; set; }
    }
}