// using Microsoft.AspNetCore.Mvc;
// using SagaStateMachine.StateMachine.Events;
//
// namespace MassTransit.POC.Controllers;
//
// [ApiController]
// [Route("[controller]")]
// public class OrdersStateMachineController : ControllerBase
// {
//     private readonly IBus _bus;
//
//     public OrdersStateMachineController(IBus bus)
//     {
//         _bus = bus;
//     }
//
//     [HttpGet("submit")]
//     public async Task<IActionResult> Submit()
//     {
//         var id = NewId.NextGuid();
//         await _bus.Publish(new SubmitOrder { OrderId = id});
//         return Ok(id);
//     }
//     
//     [HttpGet("processing/{orderId:guid}")]
//     public async Task<IActionResult> Processing(Guid orderId)
//     {
//         await _bus.Publish(new OrderProcessing { OrderId = orderId});
//         return Ok();
//     }
//     
//     [HttpGet("completed/{orderId:guid}")]
//     public async Task<IActionResult> Complete(Guid orderId)
//     {
//         await _bus.Publish(new OrderCompleted { OrderId = orderId});
//         return Ok();
//     }
//     
//     [HttpGet("cancelled/{orderId:guid}")]
//     public async Task<IActionResult> Cancel(Guid orderId)
//     {
//         await _bus.Publish(new OrderCancelled { OrderId = orderId});
//         return Ok();
//     }
// }