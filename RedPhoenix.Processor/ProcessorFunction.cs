namespace RedPhoenix.Processor;

using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Autofac;
using Infrastructure.Messages.Abstraction;
using Infrastructure.Messages.DataAnnotations;
using Infrastructure.Serialization;
using Module;
using System.Diagnostics;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

public class ProcessorFunction(ILogger<ProcessorFunction> logger)
{

    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger));
    private const int MaxProcessingRetries = 3;

    private static readonly string _instrumentationKey =
        Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING",
            EnvironmentVariableTarget.Process)!;
    

    [Function("Processor-Game-0-R")]
    public async Task RunGame0(
        [ServiceBusTrigger("generalbus-game", "Game-0-R", 
            Connection = "ServiceBusConnectionString")]
            ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {

        await ProcessorMessage(message, messageActions);
    }

    [Function("Processor-Game-1-R")]
    public async Task RunGame1(
        [ServiceBusTrigger("generalbus-game", "Game-1-R", 
            Connection = "ServiceBusConnectionString")]
            ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {

        await ProcessorMessage(message, messageActions);
    }

    [Function("Processor-Game-2-R")]
    public async Task RunGame2(
        [ServiceBusTrigger("generalbus-game", "Game-2-R", 
            Connection = "ServiceBusConnectionString")]
            ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {

        await ProcessorMessage(message, messageActions);
    }

    [Function("Processor-Game-3-R")]
    public async Task RunGame3(
        [ServiceBusTrigger("generalbus-game", "Game-3-R", 
            Connection = "ServiceBusConnectionString")]
            ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {

        await ProcessorMessage(message, messageActions);
    }

    [Function("Processor-Game-4-R")]
    public async Task RunGame4(
        [ServiceBusTrigger("generalbus-game", "Game-4-R", 
            Connection = "ServiceBusConnectionString")]
            ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        // Complete the message
        await ProcessorMessage(message, messageActions);
    }

    private async Task ProcessorMessage(ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);

        var messageBody = message.Body.ToString();

        //_logger.LogInformation("Message Body: {body}", messageBody);
        
        var userId = message.ApplicationProperties["UserId"].ToString();
        var messageType = message.ApplicationProperties["MessageType"].ToString();

        SendCustomEvent("ProcessMessage", userId, messageType, "Azure Function");

        var config = GetConfiguration();
        var container = GetContainer(config);

        try
        {
            await HandleMessage(container, messageBody);

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
        catch (Exception e)
        {
            await HandleProcessingException(message, messageActions, e);
        }

    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                "local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }


    private static IContainer GetContainer(IConfiguration config)
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule(new AppModule(GetSettings(config)));
        return builder.Build();
    }

    private static AppSettings GetSettings(IConfiguration config) =>
        new(config["ConnectionStrings:ServiceBus"]
            ?? throw new InvalidOperationException(),
            config["ConnectionStrings:TopicName"] 
                ?? throw new InvalidOperationException(),
            config["ConnectionStrings:SignalRConnectionString"] 
                ?? throw new InvalidOperationException());

    private static async Task HandleMessage(IContainer container, string value)
    {
        var serializer = container.Resolve<ITextSerializer>();

        var message = serializer.Deserialize(value) as object;
        if (message != null)
        {
            var handler = container.Resolve<IMessageHandler>();
            await handler.Handle(new Envelope(Guid.NewGuid(), message), CancellationToken.None);
        }
    }

    private async Task HandleProcessingException(
        ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, Exception e)
    {
        Trace.TraceError(message.PartitionKey + " processing Failed");
        if (message.DeliveryCount < MaxProcessingRetries)
        {
            Trace.TraceError("An error occurred while processing the message" +
                             message.MessageId + " and will be dead-lettered:\r\n{0}", e);
            await messageActions.DeadLetterMessageAsync(message, null, e.Message);
        }
        else
        {
            Trace.TraceWarning("An error occurred while processing the message" +
                               message.MessageId + " and will be abandoned:\r\n{0}", e);
            await messageActions.AbandonMessageAsync(message);
        }


    }

    private static void SendCustomEvent(string eventName, string userId,
        string subApplication, string mainApplication)
    {   
        var config = new TelemetryConfiguration
        {
            ConnectionString = _instrumentationKey
        };

        var telemetryClient = new TelemetryClient(config);

        var eventTelemetry = new EventTelemetry(eventName)
        {
            Properties =
            {
                ["UserId"] = userId,
                ["SubApplication"] = subApplication,
                ["MainApplication"] = mainApplication
            }
        };


        telemetryClient.TrackEvent(eventTelemetry);
        telemetryClient.Flush();
    }
}
