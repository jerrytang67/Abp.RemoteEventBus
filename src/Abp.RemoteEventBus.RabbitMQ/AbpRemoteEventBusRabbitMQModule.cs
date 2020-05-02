using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;

namespace Abp.RemoteEventBus.RabbitMQ
{
    [DependsOn(typeof(AbpRemoteEventBusModule))]
    public class AbpRemoteEventBusRabbitMQModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.IocContainer.Register(
                Component.For<IRabbitMqFactory>().ImplementedBy<RabbitMqFactory>()
                    .LifestyleSingleton().IsDefault()
            );

            IocManager.RegisterAssemblyByConvention(typeof(AbpRemoteEventBusRabbitMQModule).GetAssembly());
        }
    }
}