using Abp.Dependency;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Castle.Core.Logging;

namespace Abp.RemoteEventBus.RabbitMQ
{
    public interface IRabbitMqFactory : IDisposable
    {
        IModel GetChannel();
    }


    public class RabbitMqFactory : IRabbitMqFactory
    {
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;
        public ILogger Logger;

        public RabbitMqFactory(
            IRabbitMQSetting rabbitMQSetting,
            ILogger logger
        )
        {
            _logger = logger;
            
            rabbitMQSetting.Url = rabbitMQSetting.Url ?? "amqp://guest:guest@127.0.0.1:5672/";

            try
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(rabbitMQSetting.Url),
                    AutomaticRecoveryEnabled = true
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
            return this._channel;
        }


        public void Dispose()
        {
            _logger.Warn("MQ Dispose");
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}