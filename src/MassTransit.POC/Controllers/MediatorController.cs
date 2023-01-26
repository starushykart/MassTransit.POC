using MassTransit.Mediator;
using Mediator.Models;
using Microsoft.AspNetCore.Mvc;
using SagaStateMachine.StateMachine.Events;

namespace MassTransit.POC.Controllers;

[ApiController]
[Route("[controller]")]
public class MediatorController : ControllerBase
{
    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromServices]IMediator mediator)
    {
        var client = mediator.CreateRequestClient<ISubmitOrder>();
        var response = await client.GetResponse<ISubmitOrderResponse>(new { Date = DateTime.UtcNow });
        
        return Ok(response.Message.OrderId);
    }
    
    [HttpGet("process/{orderId:guid}")]
    public async Task<IActionResult> ProcessOrder([FromServices]IBus bus, Guid orderId, bool applyDiscount = false)
    {
        await bus.Publish(new ProcessOrder(orderId, applyDiscount));
        return Ok();
    }
    
    [HttpGet("approve/{orderId:guid}")]
    public async Task<IActionResult> ApproveOrder([FromServices]IBus bus, Guid orderId)
    {
        await bus.Publish(new ApproveOrder(orderId));
        return Ok();
    }
    
    public class IdModel
    {
        public Guid? Id { get; set; }
    }
}