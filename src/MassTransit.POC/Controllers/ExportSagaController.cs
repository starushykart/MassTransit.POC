using Microsoft.AspNetCore.Mvc;
using SagaConsumer.Events;

namespace MassTransit.POC.Controllers;

[ApiController]
[Route("[controller]")]
public class ExportSagaController : ControllerBase
{
    private readonly IBus _bus;

    public ExportSagaController(IBus bus)
    {
        _bus = bus;
    }

    [HttpGet("submit")]
    public async Task<IActionResult> Submit()
    {
        var id = NewId.NextGuid();
        await _bus.Publish(new SubmitExport { CorrelationId = id});
        return Ok(id);
    }

    [HttpGet("completed/{orderId:guid}")]
    public async Task<IActionResult> Complete(Guid orderId)
    {
        await _bus.Publish(new ExportCompleted { CorrelationId = orderId});
        return Ok();
    }
}