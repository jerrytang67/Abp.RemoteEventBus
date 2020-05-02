using System;
using Commons.Pool;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace Abp.RemoteEventBus.RabbitMQ
{
    public class RabbitMQRemoteEventPublisher : IRemoteEventPublisher
    {
        private const string _exchangeTopic = "RemoteEventBus.Exchange.Topic";

        private readonly IRemoteEventSerializer _remoteEventSerializer;
        private readonly ILogger _logger;
        private readonly IRabbitMqFactory _rabbitMqFactory;

        public RabbitMQRemoteEventPublisher(
            IRabbitMQSetting rabbitMQSetting,
            IRemoteEventSerializer remoteEventSerializer,
            ILogger logger,
            IRabbitMqFactory rabbitMqFactory
        )
        {
            _remoteEventSerializer = remoteEventSerializer;
            _logger = logger;
            _rabbitMqFactory = rabbitMqFactory;
        }

        public void Publish(string topic, IRemoteEventData remoteEventData)
        {
            var connection = _rabbitMqFactory;
            try
            {
                var channel = _rabbitMqFactory.GetChannel();
                channel.ExchangeDeclare(_exchangeTopic, "topic", true);
                var body = Encoding.UTF8.GetBytes(_remoteEventSerializer.Serialize(remoteEventData));
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(_exchangeTopic, topic, properties, body);
            }
            finally
            {
                // _connectionPool.Return(connection);
            }
        }

        public Task PublishAsync(string topic, IRemoteEventData remoteEventData)
        {
            return Task.Factory.StartNew(() => { Publish(topic, remoteEventData); });
        }

        public void Dispose()
        {
            _rabbitMqFactory?.Dispose();
        }
    }
}