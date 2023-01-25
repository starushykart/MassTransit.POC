using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;
using SagaStateMachine.Mediator.Models;
using SagaStateMachine.StateMachine.Events;

namespace MassTransit.POC.Controllers;

[ApiController]
[Route("[controller]")]
public class MediatorController : ControllerBase
{
    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromServices]IMediator mediator, [FromBody]IdModel model)
    {
        var client = mediator.CreateRequestClient<ISubmitOrder>();
        var response = await client.GetResponse<ISubmitOrderResponse>(new
        {
            OrderId = model.Id
        });
        
        return Ok(response.Message.Text);
    }
    
    [HttpGet("process/{orderId:guid}")]
    public async Task<IActionResult> ProcessOrder([FromServices]IBus bus, Guid orderId)
    {
        await bus.Publish(new ProcessOrder(orderId));
        return Ok();
    }
    
    public class IdModel
    {
        public Guid? Id { get; set; }
    }
}