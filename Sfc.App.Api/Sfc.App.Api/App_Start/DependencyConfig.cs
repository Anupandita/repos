using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Web.Http;
using AutoMapper;
using RestSharp;
using Sfc.App.App.Gateways;
using Sfc.App.App.Interfaces;
using Sfc.Wms.Security.Rbac.AutoMapper;
using Sfc.Wms.Security.Rbac.Services;
using Sfc.Wms.Security.Contracts.Interfaces;
using Sfc.Wms.Security.Rbac.Repository.Gateways;
using Sfc.Wms.Security.Rbac.Repository.Interfaces;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using Sfc.Wms.Data.Context;
using Sfc.Wms.Security.Rbac.UnitOfWork;
using Sfc.Wms.Security.Rbac.Interface;
using Sfc.Core.Cache.InMemory;
using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.App.Mappers;
using Sfc.Wms.Asrs.App.Services;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Repository.Gateways;
using Sfc.Wms.Asrs.Dematic.Repository.Interfaces;
using Sfc.Wms.Asrs.Shamrock.App.Interfaces;
using Sfc.Wms.Asrs.Shamrock.App.UnitOfWork;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Repository.Gateways;
using Sfc.Wms.Data.Interfaces;
using Sfc.Wms.InboundLpn.App.Services;
using Entities=Sfc.Wms.Data.Entities;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.InboundLpn.Contracts.Interfaces;
using Sfc.Wms.InboundLpn.Repository.Gateways;
using Sfc.Wms.InboundLpn.Repository.Interfaces;
using Sfc.Wms.Interface.Asrs.Interfaces;
using Sfc.Wms.NextUpCounter.Contracts.Interfaces;
using Sfc.Wms.NextUpCounter.Repository.Context;
using Sfc.Wms.NextUpCounter.Repository.Gateways;
using Sfc.Wms.NextUpCounter.Repository.Interfaces;
using Sfc.Wms.Parser.Parsers;
using Sfc.Wms.TransitionalInventory.App.Services;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.TransitionalInventory.Contracts.Interfaces;
using Sfc.Wms.TransitionalInventory.Repository.Context;
using Sfc.Wms.TransitionalInventory.Repository.Gateways;
using Sfc.Wms.TransitionalInventory.Repository.Interfaces;
using Sfc.Wms.ParserAndTranslator.Contracts.Interfaces;
using Sfc.Wms.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Translator.Interface;
using Sfc.Wms.Translator.Translators;
using Sfc.Wms.Asrs.Shamrock.Repository.Interfaces;
using Sfc.Wms.Builder.MessageBuilder;
using Sfc.Wms.NextUpCounter.App.Interfaces;
using Sfc.Wms.NextUpCounter.App.Services;
using Sfc.Wms.NextUpCounter.App.UnitOfWork;
using Sfc.Wms.UnitOfWork;

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
                    SfcRbacMapper.CreateMaps(c);
                    c.CreateMap<Entities.SwmToMhe, SwmToMheDto>().ReverseMap();
                    c.CreateMap<Entities.SwmFromMhe, SwmFromMheDto>().ReverseMap();
                    c.CreateMap<Entities.WmsToEms, WmsToEmsDto>().ReverseMap();
                    c.CreateMap<Entities.EmsToWms, EmsToWmsDto>().ReverseMap();
                    c.CreateMap<Entities.SwmMessageSource, SwmMessageSourceDto>(MemberList.None).ReverseMap();
                    c.CreateMap<Entities.CaseHeader, CaseHeaderDto>().ReverseMap();
                    c.CreateMap<Entities.CaseDetail, CaseDetailDto>().ReverseMap();
                    c.CreateMap<Entities.TransitionalInventory, TransitionalInventoryDto>().ReverseMap();
                    c.CreateMap<Entities.AllocationInventoryDetail, AllocationInventoryDetailDto>().ReverseMap();
                }));
#if DEBUG
                mapper.DefaultContext.ConfigurationProvider.AssertConfigurationIsValid();
#endif
                return mapper;
            });

            container.Register(() =>
                    new ShamrockContext(ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString)
                , Lifestyle.Singleton);
            container.Register<IRbacGateway, RbacGateway>();
            container.Register<IUserRbacUnitOfWork, UserRbacUnitOfWork>();
            container.Register<IUserRabcService, UserRabcService>();
            container.Register<IUserAuthenticationGateway, UserAuthenticationGateway>();
            container.Register<ISfcInMemoryCache>(() => new SfcInMemoryCache(MemoryCache.Default));

            container.Register(typeof(IDematicGateway<>), typeof(IDematicGateway<>).Assembly);
            container.Register(typeof(IGenericRepository<>), typeof(GenericRepository<>).Assembly);

            container.Register(typeof(IShamrockGateway<>), typeof(IShamrockGateway<>).Assembly);
            container.Register(typeof(ISwmMessageSourceGateway<>), typeof(ISwmMessageSourceGateway<>).Assembly);
            container.Register(typeof(IGenericRepository<>), typeof(IGenericRepository<>).Assembly);

           

            container.Register(() => new TransitionalInventoryContext(
                      ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString) as DbContext, Lifestyle.Singleton);
            

            container.Register<IMappingFixture>(() => new MappingFixture(), Lifestyle.Singleton);
            container.Register(typeof(IEmsToWmsService), typeof(EmsToWmsService));
            container.Register(typeof(ICaseHeaderService), typeof(CaseHeaderService));
            container.Register(typeof(ICaseDetailService), typeof(CaseDetailService));
            container.Register(typeof(IWmsToEmsService), typeof(WmsToEmsService));
            container.Register(typeof(IDematicGateway<>), typeof(DematicGateway<>));
            container.Register(typeof(IInboundLpnService), typeof(InboundLpnService));
            container.Register(typeof(ISwmFromMheService), typeof(SwmFromMheService));
            container.Register(typeof(ISwmToMheService), typeof(SwmToMheService));
            container.Register(typeof(IInboundLpnGateway<>), typeof(InboundLpnGateway<>));
            container.Register(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            container.Register(typeof(IShamrockGateway<>), typeof(ShamrockRepository<>));
            container.Register(typeof(ISwmMessageSourceGateway<>), typeof(SwmMessageSourceRepository<>));
            container.Register(typeof(IWmsToEmsMessageProcessorService), typeof(WmsToEmsMessageProcessorService));
            container.Register(typeof(IEmsToWmsMessageProcessorSevice), typeof(EmsToWmsMessageProcessorService));
            container.Register(typeof(ITransitionalInventoryService), typeof(TransitionalInventoryService));
            container.Register(typeof(ITransitionalInventoryGateway<,>), typeof(TransitionalInventoryGateway<,>));
            container.Register(typeof(IPickLocationService), typeof(PickLocationService));
            container.Register(typeof(IInboundMessageService), typeof(InboundMessageService));
            container.Register(typeof(IInboundMessageGateway), typeof(InboundMessageGateway));
            container.Register(typeof(ITaskGateway), typeof(TaskGateway));
            container.Register(typeof(ICanBuildMessage), typeof(MainMessageBuilder));
            container.Register(typeof(IMainParser), typeof(MainParser));
            container.Register(typeof(IMessageParser), typeof(MessageParser));
            container.Register(typeof(IDematicTranslatorInterface), typeof(DematicTranslator));
            container.Register(typeof(IWmsToEmsTranslator), typeof(WmsToEmsTranslator));
            container.Register(typeof(IEmsToWmsTranslator), typeof(EmsToWmsTranslator));
            container.Register(typeof(IHaveDataTypeValidation), typeof(DataTypeValidation));
            container.Register(typeof(IGenericMessageBuilder), typeof(GenericMessageBuilder));
            container.Register(typeof(ISfcUnitOfWork<,>), typeof(SfcUnitOfWork<,>));
            container.Register(typeof(IShamrockService<,>), typeof(ShamrockService<,>));
            container.Register(typeof(INextUpCounterService), typeof(NextUpCounterService));
            container.Register(typeof(INextUpCounterGateway), typeof(NextUpCounterGateway));
            container.Register(typeof(INextUpCounterRepository), typeof(NextUpCounterRepository));
            container.Register(typeof(INextUpCounterUnitOfWork), typeof(NextUpCounterUnitOfWork));
            container.Register(typeof(IAsrsUnitOfWork), typeof(AsrsUnitOfWork));

        }
    }
}