using platformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace platformService.AsyncDataServices.Messaging;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConnection? _connection;
    private readonly IModel? _channel;
    private const string _exchange = "trigger";

    public MessageBusClient(IConfiguration configuration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQHost"],
            Port = int.Parse(configuration["RabbitMQPort"]!)
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection?.CreateModel();

            _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);

            if (_connection is not null)
                _connection.ConnectionShutdown += RebbitMq_ConnectionShutdown!;

            Console.WriteLine("--> Connected to Message Bus successfully!");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Error occuired while connecting to message Bus! Trace - {ex.Message}");
        }
    }

    public async Task PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);

        if (_connection != null && _connection.IsOpen)
            await SendMessage(message);
    }

    public async Task Dispose()
    {
        if (_channel is not null && _channel.IsOpen)
            _channel.Close();
        await Task.CompletedTask;
    }

    private async Task SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: _exchange, routingKey: "", basicProperties: null, body: body);

        Console.WriteLine($"--> Message sent by RabbitMq --> Message: {message}");
        await Task.CompletedTask;
    }


    private void RebbitMq_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ connection shutdown successfully!");
    }
}
