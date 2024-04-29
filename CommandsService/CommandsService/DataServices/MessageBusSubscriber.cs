using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandsService.DataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private IConnection? _connection;
    private IModel? _channel;
    private string? _queueName;
    private const string exchangeString = "trigger";

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;

        InitializeRabbitMQ();
    }


    public override void Dispose()
    {
        if (_channel is not null && _channel.IsOpen)
        {
            _channel.Close();
            _connection?.Close();
        }
        base.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (ModuleHandle, ea) =>
        {
            Console.WriteLine(" --> Event Recived!");

            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            await _eventProcessor.ProcessEvent(notificationMessage);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"]!)
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: exchangeString, type: ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queueName, exchange: exchangeString, routingKey: string.Empty);

        Console.WriteLine($"--> Command Service is listening on the message Bus ...");

        _connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;
    }

    private void RabbitMq_ConnectionShutdown(object? sender, EventArgs e)
    {
        Console.WriteLine("--> Connection shutdown!");
    }

}
