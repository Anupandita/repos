using System.Configuration;
using System.Runtime.Caching;
using System.Web.Http;
using AutoMapper;
using Sfc.App.Gateways;
using Sfc.App.Interfaces;
using Sfc.Wms.Cache.InMemory;
using Sfc.Wms.Security.Rbac.App.AutoMapper;
using Sfc.Wms.Security.Rbac.App.Gateway;
using Sfc.Wms.Security.Rbac.App.Interfaces;
using Sfc.Wms.Security.Rbac.App.Services;
using Sfc.Wms.Security.Rbac.Contracts.Interfaces;
using Sfc.Wms.Security.Rbac.Repository.Context;
using Sfc.Wms.Security.Rbac.Repository.Gateways;
using Sfc.Wms.Security.Rbac.Repository.Interfaces;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace Sfc.App.Api.App_Start
{
    public static class DependencyConfig
    {
        public static Container Register()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            RegisterTypes(container);
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
#if DEBUG
            container.Verify();
#endif
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            return container;
        }


        private static void RegisterTypes(Container container)
        {
            container.RegisterSingleton<IMapper>(() =>
            {
              
                var mapper = new Mapper(new MapperConfiguration(c =>
                {
                SfcRbacMapper.AddProfiles(c);
                }));
#if DEBUG
                mapper.DefaultContext.ConfigurationProvider.AssertConfigurationIsValid();
#endif
                return mapper;
            });

            container.Register(() =>
                    new SfcRbacContext(ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString)
                , Lifestyle.Singleton);
            container.Register<IRbacGateway, RbacGateway>();
            container.Register<IUserRabcService, UserRabcService>();
            container.Register<IUserRabcGateway, UserRabcGateway>();
            container.Register<IUserAuthenticationGateway, UserAuthenticationGateway>();
            container.Register<ISfcInMemoryCache>(() => new SfcInMemoryCache(MemoryCache.Default));
        }
    }
}