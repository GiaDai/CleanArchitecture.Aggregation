using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
//RunContructorInjection.Run();
//RunPropertyInjection.Run();
//RunMethodInjection.Run();


string _queueName = "queue";
string _hostName = "amqp://vietbank:123456789@localhost/";
var factory = new ConnectionFactory() { Uri = new Uri(_hostName) };
IConnection _connection = factory.CreateConnection();
IModel _channel = _connection.CreateModel();
_channel.QueueDeclare(queue: _queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );
var consumer = new EventingBasicConsumer(_channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"[x] Received '{message}'");
};

_channel.BasicConsume(queue: _queueName,
                        autoAck: true,
                        consumer: consumer);
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
_channel.Close();
_connection.Close();