using System.Configuration;
using System.Runtime.Caching;
using System.Web.Http;
using AutoMapper;
using Sfc.App.App.Gateways;
using Sfc.App.App.Interfaces;
using Sfc.Core.Cache.InMemory;
using Sfc.Core.OnPrem.Security.Contracts.Extensions;
using Sfc.Core.OnPrem.Security.Contracts.Interfaces;
using Sfc.Wms.Data.Context;
using Sfc.Wms.Security.Rbac.AutoMapper;
using Sfc.Wms.Security.Rbac.Interface;
using Sfc.Wms.Security.Rbac.Repository.Gateways;
using Sfc.Wms.Security.Rbac.Repository.Interfaces;
using Sfc.Wms.Security.Rbac.Services;
using Sfc.Wms.Security.Rbac.UnitOfWork;
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
                var mapper = new Mapper(new MapperConfiguration(c => { SfcRbacMapper.CreateMaps(c); }));
#if DEBUG
                mapper.DefaultContext.ConfigurationProvider.AssertConfigurationIsValid();
#endif
                return mapper;
            });

            container.Register(() =>
                    new ShamrockContext(ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString)
                , Lifestyle.Singleton);
            container.Register(() =>
                    ConfigurationManager.AppSettings["db:encryptionKey"].ToSecureString()
                , Lifestyle.Singleton);
            container.Register<IRbacGateway, RbacGateway>();
            container.Register<IUserRbacUnitOfWork, UserRbacUnitOfWork>();
            container.Register<IUserRbacService, UserRbacService>();
            container.Register<IUserAuthenticationGateway, UserAuthenticationGateway>();
            container.Register<ISfcInMemoryCache>(() => new SfcInMemoryCache(MemoryCache.Default));
        }
    }
}