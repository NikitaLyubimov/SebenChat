using Core.Interfaces.UseCases;
using Core.UseCases;
using Core.Helpers;

using Autofac;

namespace Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisterUserUsecase>().As<IRegisterUserUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<LoginUserUseCase>().As<ILoginUserUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<RefreshTokenUseCase>().As<IRefreshTokenUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<EmailActions>();
        }
    }
}
