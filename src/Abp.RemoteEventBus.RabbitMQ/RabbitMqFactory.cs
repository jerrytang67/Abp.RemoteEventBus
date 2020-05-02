using RabbitMQ.Client;
using System;
using Castle.Core.Logging;
using Microsoft.Extensions.Options;

namespace Abp.RemoteEventBus.RabbitMQ
{
    public interface IRabbitMqFactory : IDisposable
    {
        IModel GetChannel();
    }

    public class RabbitMQOptions
    {
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string HostName { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5672;

        public string QueryName { get; set; } = "Default_QueryName";
        public bool AutomaticRecoveryEnabled { get; set; } = true;
    }

    public class RabbitMqFactory : IRabbitMqFactory
    {
        private readonly RabbitMQOptions _options;
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqFactory(
            RabbitMQOptions options,
            ILogger logger
        )
        {
            _options = options;
            _logger = logger;

            try
            {
                var factory = new ConnectionFactory()
                {
                    UserName = _options.UserName,
                    Password = _options.Password,
                    HostName = _options.HostName,
                    Port = _options.Port,
                    AutomaticRecoveryEnabled = _options.AutomaticRecoveryEnabled
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _logger.Warn($"RabbitMQ Client 连接成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitListener init error,ex:{ex.Message}");
            }
        }

        public IModel GetChannel()
        {
            return _channel;
        }

        public void Dispose()
        {
            _logger.Warn("MQ Dispose");
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}