using Core.Interfaces.Gateways.Reposytories;
using Core.Interfaces.Services;
using Infrustructure.Tokens;
using Infrustructure.Data.Repositories;

using Autofac;
using Module = Autofac.Module;

namespace Infrustructure
{
    public class InfrustructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserReposytory>().As<IUserReposytory>().InstancePerLifetimeScope();
            builder.RegisterType<EmailTokenReposytory>().As<IEmailTokenReposytory>().InstancePerLifetimeScope();
            builder.RegisterType<MessagesReposytory>().As<IMessageReposytory>().InstancePerLifetimeScope();
            builder.RegisterType<JwtFactory>().As<IJwtFactory>().InstancePerLifetimeScope();
            builder.RegisterType<TokenFactory>().As<ITokenFactory>().InstancePerMatchingLifetimeScope();
            builder.RegisterType<JwtTokenValidator>().As<IJwtTokenValidator>().InstancePerMatchingLifetimeScope();
        }
    }
}
