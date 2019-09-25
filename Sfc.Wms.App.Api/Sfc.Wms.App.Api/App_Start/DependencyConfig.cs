using System.Configuration;
using System.Runtime.Caching;
using System.Web.Http;
using AutoMapper;
using Sfc.Core.Cache.InMemory;
using Sfc.Core.OnPrem.Security.Contracts.Extensions;
using Sfc.Core.OnPrem.Security.Contracts.Interfaces;
using Sfc.Wms.App.Api.Interfaces;
using Sfc.Wms.App.Api.Services;
using Sfc.Wms.App.App.AutoMapper;
using Sfc.Wms.App.App.Gateways;
using Sfc.Wms.App.App.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;
using Sfc.Wms.Configuration.SystemCode.Contracts.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Repository.Context;
using Sfc.Wms.Configuration.SystemCode.Repository.Dtos;
using Sfc.Wms.Configuration.SystemCode.Repository.Gateways;
using Sfc.Wms.Configuration.SystemCode.Repository.Interfaces;
using Sfc.Wms.Data.Context;
using Sfc.Wms.Data.Interfaces;
using Sfc.Wms.Framework.Security.Rbac.AutoMapper;
using Sfc.Wms.Framework.Security.Rbac.Interface;
using Sfc.Wms.Framework.Security.Rbac.Services;
using Sfc.Wms.Framework.Security.Rbac.UnitOfWork;
using Sfc.Wms.Security.Rbac.Repository.Gateways;
using Sfc.Wms.Security.Rbac.Repository.Interfaces;
using Sfc.Wms.SystemCode.App.AutoMapper;
using Sfc.Wms.SystemCode.App.Interfaces;
using Sfc.Wms.SystemCode.App.Services;
using Sfc.Wms.SystemCode.App.UnitOfWork;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace Sfc.Wms.App.Api
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
                var mapper = new Mapper(new MapperConfiguration(c => { SfcRbacMapper.CreateMaps(c); SystemCodeAutoMapper.CreateMaps(c); PrinterValuesMapper.CreateMaps(c); }));
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

            container.Register(typeof(ISystemCodeService), typeof(SystemCodeService));
            container.Register(typeof(ISystemCodeUnitOfWork), typeof(SystemCodeUnitOfWork));
            container.Register(typeof(ISystemCodeRepositoryGateway), typeof(SystemCodeRepositoryGateway));
            container.Register(typeof(ISystemCodeRepository), typeof(SystemCodeRepository));
            container.Register(typeof(IGenericRepository<SysCode>), typeof(GenericRepository<SysCode>));
            container.Register(typeof(IGenericRepository<WhseSysCode>), typeof(GenericRepository<WhseSysCode>));
            container.Register(typeof(IGenericRepository<SysCodeType>), typeof(GenericRepository<SysCodeType>));
            container.Register(typeof(IRbacService), typeof(RbacService));
        }
    }
}   