using Marten.Schema;
using MassTransit.Saga.StateMachineSaga.Events;

namespace MassTransit.Saga.ConsumerSaga;

public class ExportSaga : 
    ISaga,
    InitiatedBy<SubmitExport>,
    Orchestrates<ExportCompleted>
{
    [Identity]
    public Guid CorrelationId { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    public Task Consume(ConsumeContext<SubmitExport> context)
    {
        //save to database
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<ExportCompleted> context)
    {
        CompletedAt = DateTime.UtcNow;
        
        // finalize saga. will be deleted from db
        // var sagaContext = context as SagaConsumeContext<ExportSaga, ExportCompleted>;
        // sagaContext?.SetCompleted();
        
        return Task.CompletedTask;
    }
}