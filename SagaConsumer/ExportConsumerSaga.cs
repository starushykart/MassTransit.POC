using Marten.Schema;
using MassTransit;
using SagaConsumer.Events;

namespace SagaConsumer;

public class ExportConsumerSaga : 
    ISaga,
    InitiatedBy<SubmitExport>,
    Orchestrates<ExportCompleted>
{
    [Identity]
    public Guid CorrelationId { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    public Task Consume(ConsumeContext<SubmitExport> context)
    {
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<ExportCompleted> context)
    {
        CompletedAt = DateTime.UtcNow;
        
        // finalize saga. will be deleted from db
        // var sagaContext = context as SagaConsumeContext<ExportConsumerSaga, ExportCompleted>;
        // sagaContext.SetCompleted();
        
        return Task.CompletedTask;
    }
}