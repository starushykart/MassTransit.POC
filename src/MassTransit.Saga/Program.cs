using Amazon.SimpleNotificationService;
using Amazon.SQS;
using MassTransit;
using MassTransit.Saga.ConsumerSaga;
using MassTransit.Saga.StateMachineSaga;
using Weasel.Postgresql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(cfg =>
{
    // state machine
    cfg.AddSagaStateMachine<OrderSaga, OrderState>()
        .MartenRepository(builder.Configuration.GetConnectionString("Database"), opt =>
        {
            opt.Schema.For<OrderState>().UseOptimisticConcurrency(true);
            opt.AutoCreateSchemaObjects = AutoCreate.All;
        });
    
    //consumer saga
    cfg.AddSaga<ExportSaga>()
        .MartenRepository(builder.Configuration.GetConnectionString("Database"), opt =>
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