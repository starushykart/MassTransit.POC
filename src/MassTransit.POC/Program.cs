using System.Reflection;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using FluentValidation;
using MassTransit;
using Mediator;
using Mediator.Events;
using Mediator.Filters;
using Weasel.Postgresql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssembly(typeof(AcceptMediatorActionValidator).Assembly);

builder.Services.AddMediator(x =>
{
    x.AddConsumers(typeof(MediatorActionConsumer).Assembly);
    
    x.ConfigureMediator((context, cfg) =>
    {
        cfg.UseSendFilter(typeof(ValidationFilter<>), context);
    });
});

builder.Services.AddMassTransit(cfg =>
{
    var assembly = Assembly.GetExecutingAssembly();
    
    cfg.AddSagaStateMachines(assembly);
    cfg.AddSagas(assembly);
    cfg.AddConsumers(assembly);

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