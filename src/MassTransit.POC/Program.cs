using Amazon.SimpleNotificationService;
using Amazon.SQS;
using FluentValidation;
using MassTransit;
using Mediator;
using Mediator.Filters;
using Mediator.Models;
using SagaConsumer;
using SagaStateMachine.StateMachine;
using Weasel.Postgresql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssembly(typeof(SubmitOrderValidator).Assembly);

builder.Services.AddMediator(x =>
{
    x.AddConsumers(typeof(SubmitOrderConsumer).Assembly);

    x.ConfigureMediator((context, cfg) =>
    {
        cfg.UseSendFilter(typeof(ValidationFilter<>), context);
    });
});

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddSagas(typeof(ExportConsumerSaga).Assembly);
    cfg.AddActivities(typeof(OrderCourierStateMachine).Assembly);
    cfg.AddSagaStateMachines(typeof(OrderCourierStateMachine).Assembly);

    cfg.SetMartenSagaRepositoryProvider(builder.Configuration.GetConnectionString("Database"), opt =>
    {
        opt.AutoCreateSchemaObjects = AutoCreate.All;
    });
    
    cfg.AddDelayedMessageScheduler();
    cfg.UsingAmazonSqs((context, x) =>
    {
        x.UseDelayedMessageScheduler();
        var url = new Uri(builder.Configuration.GetValue<string>("LocalstackUrl"));
        
        x.Host(new Uri($"amazonsqs://{url.Authority}"), h =>
        {
            h.AccessKey("admin");
            h.SecretKey("admin");
            
            h.Config(new AmazonSQSConfig { ServiceURL = url.ToString() });
            h.Config(new AmazonSimpleNotificationServiceConfig { ServiceURL = url.ToString() });
        });
        x.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();