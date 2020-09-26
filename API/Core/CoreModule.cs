using Core.Interfaces.UseCases;
using Core.UseCases;
using Core.Helpers;

using Autofac;
using Core.Interfaces.Helpers;

namespace Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisterUserUsecase>().As<IRegisterUserUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<LoginUserUseCase>().As<ILoginUserUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<RefreshTokenUseCase>().As<IRefreshTokenUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<VerifyEmailTokenUseCase>().As<IVerifyEmailTokenUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<PublickKeyUseCase>().As<IPublickKeyUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<EmailActions>().As<IEmailActions>();
        }
    }
}
