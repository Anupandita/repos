using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using RestSharp;
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
using Sfc.Wms.Asrs.Shamrock.Repository.Interfaces;
using Sfc.Wms.Builder.MessageBuilder;
using Sfc.Wms.Data.Context;
using Sfc.Wms.Data.Entities;
using Sfc.Wms.Data.Interfaces;
using Sfc.Wms.InboundLpn.App.Services;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.InboundLpn.Contracts.Interfaces;
using Sfc.Wms.InboundLpn.Repository.Gateways;
using Sfc.Wms.InboundLpn.Repository.Interfaces;
using Sfc.Wms.NextUpCounter.App.Services;
using Sfc.Wms.NextUpCounter.Contracts.Interfaces;
using Sfc.Wms.NextUpCounter.Repository.Context;
using Sfc.Wms.NextUpCounter.Repository.Gateways;
using Sfc.Wms.NextUpCounter.Repository.Interfaces;
using Sfc.Wms.Parser.Parsers;
using Sfc.Wms.ParserAndTranslator.Contracts.Interfaces;
using Sfc.Wms.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.TransitionalInventory.App.Services;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.TransitionalInventory.Contracts.Interfaces;
using Sfc.Wms.TransitionalInventory.Repository.Context;
using Sfc.Wms.TransitionalInventory.Repository.Gateways;
using Sfc.Wms.TransitionalInventory.Repository.Interfaces;
using Sfc.Wms.Translator.Interface;
using Sfc.Wms.Translator.Translators;
using Sfc.Wms.UnitOfWork;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Web.Http;
using SfcMapper = Sfc.Wms.Asrs.App.Mappers.SfcMapper;
using TransDtos = Sfc.Wms.TransitionalInventory.Repository.Dtos;

namespace Sfc.Wms.Asrs.Api.App_Start
{
    public static class DependencyConfig
    {
        public static Container Register()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            RegisterTypes(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            SfcMapper.Initialize();

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
                var mapper = new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.AddExpressionMapping();
                    cfg.CreateMap<SwmToMhe, SwmToMheDto>().ReverseMap();
                    cfg.CreateMap<SwmFromMhe, SwmFromMheDto>().ReverseMap();
                    cfg.CreateMap<Data.Entities.WmsToEms, WmsToEmsDto>().ReverseMap();
                    cfg.CreateMap<Data.Entities.EmsToWms, EmsToWmsDto>().ReverseMap();
                    cfg.CreateMap<SwmMessageSource, SwmMessageSourceDto>(MemberList.None).ReverseMap();
                    cfg.CreateMap<CaseHeader, CaseHeaderDto>().ReverseMap();
                    cfg.CreateMap<Expression<Func<CaseHeaderDto, bool>>, Expression<Func<CaseHeader, bool>>>(MemberList.None);
                    cfg.CreateMap<Expression<Func<CaseDetailDto, bool>>, Expression<Func<CaseDetail, bool>>>(MemberList.None);
                    cfg.CreateMap<CaseDetail, CaseDetailDto>().ReverseMap();
                    cfg.CreateMap<TransDtos.TransitionalInventory, TransitionalInventoryDto>().ReverseMap();
                    cfg.CreateMap<AllocationInventoryDetail, AllocationInventoryDetailDto>().ReverseMap();
                }));
#if DEBUG
                mapper.DefaultContext.ConfigurationProvider.AssertConfigurationIsValid();
#endif
                return mapper;
            });

            container.Register(typeof(IDematicGateway<>), typeof(IDematicGateway<>).Assembly);
            container.Register(typeof(IGenericRepository<>), typeof(GenericRepository<>).Assembly);

            container.Register(typeof(IShamrockGateway<>), typeof(IShamrockGateway<>).Assembly);
            container.Register(typeof(ISwmMessageSourceGateway<>), typeof(ISwmMessageSourceGateway<>).Assembly);
            container.Register(typeof(IGenericRepository<>), typeof(IGenericRepository<>).Assembly);

            container.Register(() => new ShamrockContext(ConfigurationManager.ConnectionStrings["ShamrockDbConnection"].ConnectionString)
                , Lifestyle.Scoped);

            container.Register(() => new TransitionalInventoryContext(
                      ConfigurationManager.ConnectionStrings["ShamrockDbConnection"].ConnectionString) as DbContext, Lifestyle.Singleton);
            container.Register(() => new NextUpCounterDbContext(
                   ConfigurationManager.ConnectionStrings["ShamrockDbConnection"].ConnectionString),
               Lifestyle.Singleton);

            container.Register<IMappingFixture>(() => new App.Mappers.MappingFixture(), Lifestyle.Singleton);
            container.Register(typeof(IEmsToWmsService), typeof(EmsToWmsService));
            container.Register(typeof(IWmsToEmsService), typeof(WmsToEmsService));
            container.Register(typeof(IDematicGateway<>), typeof(DematicGateway<>));
            container.Register(typeof(IInboundLpnService), typeof(InboundLpnService));
            container.Register(typeof(ISwmFromMheService), typeof(SwmFromMheService));
            container.Register(typeof(ISwmToMheService), typeof(SwmToMheService));
            container.Register(typeof(ICaseDetailService), typeof(CaseDetailService));
            container.Register(typeof(ICaseHeaderService), typeof(CaseHeaderService));
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
            container.Register<IRestClient>(() => new RestClient(), Lifestyle.Singleton);
            container.Register(typeof(ISfcUnitOfWork<,>), typeof(SfcUnitOfWork<,>));
            container.Register(typeof(IShamrockService<,>), typeof(ShamrockService<,>));
            container.Register(typeof(INextUpCounterService), typeof(NextUpCounterService));
            container.Register(typeof(INextUpCounterGateway), typeof(NextUpCounterGateway));
            container.Register(typeof(INextUpCounterRepository), typeof(NextUpCounterRepository));
            container.Register(typeof(IAsrsUnitOfWork), typeof(AsrsUnitOfWork));
        }
    }
}