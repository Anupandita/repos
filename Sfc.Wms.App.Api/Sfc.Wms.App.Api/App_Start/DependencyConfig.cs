using AutoMapper;
using Sfc.Core.Cache.Contracts;
using Sfc.Core.Cache.InMemory;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Extensions;
using Sfc.Core.OnPrem.Security.Contracts.Interfaces;
using Sfc.Wms.App.Api.Interfaces;
using Sfc.Wms.App.Api.Services;
using Sfc.Wms.App.App.Gateways;
using Sfc.Wms.App.App.Interfaces;
using Sfc.Wms.Configuration.Security.Rbac.Repository.Gateways;
using Sfc.Wms.Configuration.Security.Rbac.Repository.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;
using Sfc.Wms.Configuration.SystemCode.Contracts.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Repository.Gateways;
using Sfc.Wms.Configuration.SystemCode.Repository.Interfaces;
using Sfc.Wms.Data.Context;
using Sfc.Wms.Data.Entities;
using Sfc.Wms.Data.Interfaces;
using Sfc.Wms.Framework.Security.Rbac.AutoMapper;
using Sfc.Wms.Framework.Security.Rbac.Interface;
using Sfc.Wms.Framework.Security.Rbac.Services;
using Sfc.Wms.Framework.Security.Rbac.UnitOfWork;
using Sfc.Wms.InboundLpn.App.Interfaces;
using Sfc.Wms.InboundLpn.App.Services;
using Sfc.Wms.InboundLpn.App.UnitOfWork;
using Sfc.Wms.Framework.SystemCode.App.Interfaces;
using Sfc.Wms.Framework.SystemCode.App.Services;
using Sfc.Wms.Framework.SystemCode.App.UnitOfWork;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Configuration;
using System.Runtime.Caching;
using System.Web.Http;
using Sfc.Wms.Foundation.InboundLpn.Repository.Interfaces;
using Sfc.Wms.Foundation.InboundLpn.Repository.Gateways;
using Sfc.Wms.Foundation.InboundLpn.Repository.Context;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Interfaces;

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
                var mapper = new Mapper(new MapperConfiguration(c =>
                {
                    SfcRbacMapper.CreateMaps(c);
                    c.CreateMap<SysCode, SysCodeDto>()
                        .ForMember(d => d.CodeId, s => s.MapFrom(p => p.CodeId))
                        .ForMember(d => d.CodeDesc, s => s.MapFrom(p => p.CodeDesc))
                        .ForMember(d => d.MiscFlag, s => s.MapFrom(p => p.MiscFlags))
                        .ForMember(d => d.ShortDesc, s => s.MapFrom(p => p.ShortDesc))
                        .ForAllOtherMembers(p => p.Ignore());

                    c.CreateMap<SysCodeDto, SfcPrinterSelectList>()
                        .ForMember(d => d.Id, s => s.MapFrom(p => p.CodeId))
                        .ForMember(d => d.Description, s => s.MapFrom(p => p.CodeDesc))
                        .ForMember(d => d.DisplayName, s => s.MapFrom(p => p.MiscFlag));
                }));
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
            container.Register<ISfcCache>(() => new SfcInMemoryCache(MemoryCache.Default));

            container.Register(typeof(ISystemCodeService), typeof(SystemCodeService));
            container.Register(typeof(ISystemCodeUnitOfWork), typeof(SystemCodeUnitOfWork));
            container.Register(typeof(ISystemCodeRepositoryGateway), typeof(SystemCodeRepositoryGateway));
            container.Register(typeof(IGenericRepository<SysCode>), typeof(GenericRepository<SysCode>));
            container.Register(typeof(IGenericRepository<WhseSysCode>), typeof(GenericRepository<WhseSysCode>));
            container.Register(typeof(IGenericRepository<SysCodeType>), typeof(GenericRepository<SysCodeType>));
            container.Register(typeof(IRbacService), typeof(RbacService));

            container.Register(typeof(IInboundLpnGenericGateway<CaseHeader>), typeof(InboundLpnGenericGateway<CaseHeader>));
            container.Register(typeof(IInboundLpnGenericGateway<CaseDetail>), typeof(InboundLpnGenericGateway<CaseDetail>));
            container.Register(typeof(IInboundLpnGenericGateway<CaseComment>), typeof(InboundLpnGenericGateway<CaseComment>));
            container.Register(typeof(IInboundLpnGenericGateway<CaseLock>), typeof(InboundLpnGenericGateway<CaseLock>));
            container.Register(typeof(IGenericRepository<CaseHeader>), typeof(GenericRepository<CaseHeader>));
            container.Register(typeof(IGenericRepository<CaseDetail>), typeof(GenericRepository<CaseDetail>));
            container.Register(typeof(IGenericRepository<CaseComment>), typeof(GenericRepository<CaseComment>));
            container.Register(typeof(IGenericRepository<CaseLock>), typeof(GenericRepository<CaseLock>));

            container.Register(typeof(IInboundLpnGateway), typeof(InboundLpnGateway));
            container.Register(typeof(IInboundMessageGateway), typeof(InboundMessageGateway));
            container.Register(typeof(IInboundLpnUnitOfWork), typeof(InboundLpnUnitOfWork));
            container.Register(typeof(IInboundLpnRepository), typeof(InboundLpnRepository));
            container.Register(typeof(IFindLpnService), typeof(FindLpnService));
            container.Register(typeof(ILpnHistoryService), typeof(LpnHistoryService));
        }
    }
}   