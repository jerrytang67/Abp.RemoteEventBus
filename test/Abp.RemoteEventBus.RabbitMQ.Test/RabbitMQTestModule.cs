using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Abp.RemoteEventBus.RabbitMQ.Test
{
    [DependsOn(typeof(AbpRemoteEventBusRabbitMQModule))]
    public class RabbitMQTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            base.PreInitialize();

            // RabbitMq 配置修改方式
            Configuration.Get<RabbitMQOptions>().HostName = "127.0.0.1";
            Configuration.Get<RabbitMQOptions>().UserName = "guest";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RabbitMQTestModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            Configuration.Modules.RemoteEventBus().UseRabbitMQ().Configure(setting => { setting.Url = "amqp://guest:guest@127.0.0.1:5672/"; });

            Configuration.Modules.RemoteEventBus().AutoSubscribe();
        }
    }
}