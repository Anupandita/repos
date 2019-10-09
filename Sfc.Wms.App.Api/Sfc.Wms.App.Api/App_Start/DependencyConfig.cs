using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Sfc.Core.Cache.Contracts;
using Sfc.Core.Cache.InMemory;
using Sfc.Core.OnPrem.LabelGenerator.Interfaces;
using Sfc.Core.OnPrem.LabelGenerator.LabelGenerators;
using Sfc.Core.OnPrem.Security.Contracts.Extensions;
using Sfc.Core.OnPrem.Security.Contracts.Interfaces;
using Sfc.Wms.App.Api.Interfaces;
using Sfc.Wms.App.Api.Services;
using Sfc.Wms.App.App.AutoMapper;
using Sfc.Wms.App.App.Gateways;
using Sfc.Wms.App.App.Interfaces;
using Sfc.Wms.Configuration.DockDoor.Contracts.Dtos;
using Sfc.Wms.Configuration.DockDoor.Contracts.Interfaces;
using Sfc.Wms.Configuration.DockDoor.Repository.Gateway;
using Sfc.Wms.Configuration.DockDoor.Repository.Interfaces;
using Sfc.Wms.Configuration.NextUpCounter.Contracts.Interfaces;
using Sfc.Wms.Configuration.NextUpCounter.Repository.Gateways;
using Sfc.Wms.Configuration.NextUpCounter.Repository.Interfaces;
using Sfc.Wms.Configuration.Security.Rbac.Repository.Gateways;
using Sfc.Wms.Configuration.Security.Rbac.Repository.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Contracts.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Repository.Context;
using Sfc.Wms.Configuration.SystemCode.Repository.Gateways;
using Sfc.Wms.Configuration.SystemCode.Repository.Interfaces;
using Sfc.Wms.Data.Context;
using Sfc.Wms.Data.Entities;
using Sfc.Wms.Data.Interfaces;
using Sfc.Wms.Foundation.AllocationInventory.App.Interfaces;
using Sfc.Wms.Foundation.AllocationInventory.App.Services;
using Sfc.Wms.Foundation.AllocationInventory.App.UnitOfWork;
using Sfc.Wms.Foundation.AllocationInventory.Contracts.Dtos;
using Sfc.Wms.Foundation.AllocationInventory.Contracts.Interfaces;
using Sfc.Wms.Foundation.AllocationInventory.Repository.Gateway;
using Sfc.Wms.Foundation.AllocationInventory.Repository.Interfaces;
using Sfc.Wms.Foundation.Carton.App.Interfaces;
using Sfc.Wms.Foundation.Carton.App.Services;
using Sfc.Wms.Foundation.Carton.App.UnitOfWork;
using Sfc.Wms.Foundation.Carton.Contracts.Dtos;
using Sfc.Wms.Foundation.Carton.Contracts.Interfaces;
using Sfc.Wms.Foundation.Carton.Repository.Gateway;
using Sfc.Wms.Foundation.Carton.Repository.Interfaces;
using Sfc.Wms.Foundation.InboundLpn.Repository.Dtos;
using Sfc.Wms.Foundation.InboundLpn.Repository.Gateways;
using Sfc.Wms.Foundation.InboundLpn.Repository.Interfaces;
using Sfc.Wms.Foundation.Location.App.Interfaces;
using Sfc.Wms.Foundation.Location.App.Services;
using Sfc.Wms.Foundation.Location.App.UnitOfWork;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Wms.Foundation.Location.Contracts.Interfaces;
using Sfc.Wms.Foundation.Location.Repository.Gateway;
using Sfc.Wms.Foundation.Location.Repository.Interfaces;
using Sfc.Wms.Foundation.Message.Contracts.Dtos;
using Sfc.Wms.Foundation.Message.Contracts.Interfaces;
using Sfc.Wms.Foundation.Message.Repository.Gateway;
using Sfc.Wms.Foundation.Message.Repository.Interfaces;
using Sfc.Wms.Foundation.MessageSource.Contracts.Dtos;
using Sfc.Wms.Foundation.MessageSource.Contracts.Interfaces;
using Sfc.Wms.Foundation.MessageSource.Repository.Gateways;
using Sfc.Wms.Foundation.MessageSource.Repository.Interfaces;
using Sfc.Wms.Foundation.PixTransaction.Contracts.Dtos;
using Sfc.Wms.Foundation.PixTransaction.Contracts.Interfaces;
using Sfc.Wms.Foundation.PixTransaction.Repository.Gateway;
using Sfc.Wms.Foundation.PixTransaction.Repository.Interfaces;
using Sfc.Wms.Foundation.SkuInventory.App.Interfaces;
using Sfc.Wms.Foundation.SkuInventory.App.Services;
using Sfc.Wms.Foundation.SkuInventory.App.UnitOfWork;
using Sfc.Wms.Foundation.SkuInventory.Contracts.Dtos;
using Sfc.Wms.Foundation.SkuInventory.Contracts.Interfaces;
using Sfc.Wms.Foundation.SkuInventory.Repository.Gateway;
using Sfc.Wms.Foundation.SkuInventory.Repository.Interfaces;
using Sfc.Wms.Foundation.Tasks.Contracts.Dtos;
using Sfc.Wms.Foundation.Tasks.Contracts.Interfaces;
using Sfc.Wms.Foundation.Tasks.Repository.Gateways;
using Sfc.Wms.Foundation.Tasks.Repository.Interfaces;
using Sfc.Wms.Foundation.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.Foundation.TransitionalInventory.Contracts.Interfaces;
using Sfc.Wms.Foundation.TransitionalInventory.Repository.Gateways;
using Sfc.Wms.Foundation.TransitionalInventory.Repository.Interfaces;
using Sfc.Wms.Framework.DockDoor.App.Interfaces;
using Sfc.Wms.Framework.DockDoor.App.Services;
using Sfc.Wms.Framework.DockDoor.App.UnitOfWork;
using Sfc.Wms.Framework.NextUpCounter.App.Interfaces;
using Sfc.Wms.Framework.NextUpCounter.App.Services;
using Sfc.Wms.Framework.NextUpCounter.App.UnitOfWork;
using Sfc.Wms.Framework.PixTransaction.App.Interfaces;
using Sfc.Wms.Framework.PixTransaction.App.Services;
using Sfc.Wms.Framework.PixTransaction.App.UnitOfWork;
using Sfc.Wms.Framework.Security.Rbac.AutoMapper;
using Sfc.Wms.Framework.Security.Rbac.Interface;
using Sfc.Wms.Framework.Security.Rbac.Services;
using Sfc.Wms.Framework.Security.Rbac.UnitOfWork;
using Sfc.Wms.Framework.SystemCode.App.Interfaces;
using Sfc.Wms.Framework.SystemCode.App.Services;
using Sfc.Wms.Framework.SystemCode.App.UnitOfWork;
using Sfc.Wms.Inbound.Tasks.App.Interfaces;
using Sfc.Wms.Inbound.Tasks.App.Services;
using Sfc.Wms.Inbound.Tasks.App.UnitOfWork;
using Sfc.Wms.Inbound.TransitionalInventory.App.Interfaces;
using Sfc.Wms.Inbound.TransitionalInventory.App.Services;
using Sfc.Wms.Inbound.TransitionalInventory.App.UnitOfWork;
using Sfc.Wms.InboundLpn.App.Interfaces;
using Sfc.Wms.InboundLpn.App.Services;
using Sfc.Wms.InboundLpn.App.UnitOfWork;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.InboundLpn.Contracts.Interfaces;
using Sfc.Wms.InboundLpn.Repos.Context;
using Sfc.Wms.InboundLpn.Repos.Gateways;
using Sfc.Wms.InboundLpn.Repos.Interfaces;
using Sfc.Wms.Interfaces.Asrs.App.Interfaces;
using Sfc.Wms.Interfaces.Asrs.App.Mappers;
using Sfc.Wms.Interfaces.Asrs.App.Services;
using Sfc.Wms.Interfaces.Asrs.App.UnitOfWork;
using Sfc.Wms.Interfaces.Asrs.Contracts.Interfaces;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Interfaces;
using Sfc.Wms.Interfaces.Asrs.Dematic.Repository.Gateways;
using Sfc.Wms.Interfaces.Asrs.Dematic.Repository.Interfaces;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Interfaces;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Repository.Gateways;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Repository.Interfaces;
using Sfc.Wms.Interfaces.Builder.MessageBuilder;
using Sfc.Wms.Interfaces.Message.App.Interfaces;
using Sfc.Wms.Interfaces.Message.App.Services;
using Sfc.Wms.Interfaces.Message.App.UnitOfWork;
using Sfc.Wms.Interfaces.MessageSource.App.Interfaces;
using Sfc.Wms.Interfaces.MessageSource.App.Services;
using Sfc.Wms.Interfaces.MessageSource.App.UnitOfWork;
using Sfc.Wms.Interfaces.Parser.Parsers;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Interfaces;
using Sfc.Wms.Interfaces.Translator.Interface;
using Sfc.Wms.Interfaces.Translator.Translators;
using Sfc.Wms.SystemCode.App.AutoMapper;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Configuration;
using System.Runtime.Caching;
using System.Web.Http;

namespace Sfc.Wms.App.Api
{
    public static class DependencyConfig
    {
        public static Container Register()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            Mapper.Reset(); SfcMapper.Initialize();
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
                var mapper = new Mapper(new MapperConfiguration(cfg =>
                    {
                        SfcRbacMapper.CreateMaps(cfg);
                        SystemCodeAutoMapper.CreateMaps(cfg);
                        PrinterValuesMapper.CreateMaps(cfg);
                        cfg.AddExpressionMapping();
                        cfg.CreateMap<InboundMessage, InboundMessageDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<TaskHeader, TaskHeaderDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<TaskDetail, TaskDetailDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<SwmToMhe, SwmToMheDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<SwmFromMhe, SwmFromMheDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<WmsToEms, WmsToEmsDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<EmsToWms, EmsToWmsDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<SwmMessageSource, SwmMessageSourceDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<CaseHeader, CaseHeaderDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<PickLocationDetail, PickLocationDetailsDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<CaseDetail, CaseDetailDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<TransitionalInventory, TransitionalInventoryDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<PixTransaction, PixTransactionDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<SkuInventory, SkuInventoryDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<LocationGroup, LocationGroupDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<CartonHeader, CartonHeaderDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<PickLocationDetailsExtenstion, PickLocationDetailsExtenstionDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<PickLocationDetailsExtenstion, PickLocationDetailsDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<DockDoorMaster, DockDoorMasterDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<LocationHeader, LocationHeaderDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<PickTicketDetail, PickTicketDetailDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<PickTicketHeader, PickTicketHeaderDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<CartonDetail, CartonDetailDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<AllocationInventoryDetail, AllocationInventoryDetailDto>(MemberList.None).ReverseMap();
                        cfg.CreateMap<MessageToSortView, MessageToSortViewDto>(MemberList.None).ReverseMap();
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
            container.Register(typeof(ISystemCodeRepository), typeof(SystemCodeRepository));
            container.Register(typeof(IRbacService), typeof(RbacService));

            container.Register<IMappingFixture>(() => new MappingFixture(), Lifestyle.Singleton);
            container.Register(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            container.Register(typeof(IAsrsUnitOfWork), typeof(AsrsUnitOfWork));
            container.Register(typeof(IDematicGateway<>), typeof(IDematicGateway<>).Assembly);
            container.Register(typeof(IGenericRepository<>), typeof(GenericRepository<>).Assembly);
            container.Register(typeof(IShamrockGateway<>), typeof(IShamrockGateway<>).Assembly);
            container.Register(typeof(IEmsToWmsService), typeof(EmsToWmsService));
            container.Register(typeof(IWmsToEmsService), typeof(WmsToEmsService));
            container.Register(typeof(IDematicGateway<>), typeof(DematicGateway<>));
            container.Register(typeof(ISwmFromMheService), typeof(SwmFromMheService));
            container.Register(typeof(ISwmToMheService), typeof(SwmToMheService));
            container.Register(typeof(IShamrockGateway<>), typeof(ShamrockRepository<>));
            container.Register(typeof(IWmsToEmsMessageProcessorService), typeof(WmsToEmsMessageProcessorService));
            container.Register(typeof(IEmsToWmsMessageProcessorSevice), typeof(EmsToWmsMessageProcessorService));

            container.Register(typeof(IBuildMessage), typeof(MainMessageBuilder));
            container.Register(typeof(IParseMessage), typeof(MainParser));
            container.Register(typeof(ITranslateDematicMessage), typeof(DematicTranslator));
            container.Register(typeof(IWmsToEmsTranslator), typeof(WmsToEmsTranslator));
            container.Register(typeof(IEmsToWmsTranslator), typeof(EmsToWmsTranslator));

            container.Register(typeof(IInboundLpnUnitOfWork), typeof(InboundLpnUnitOfWork));
            container.Register(typeof(ICaseHeaderService), typeof(CaseHeaderService));
            container.Register(typeof(ICaseDetailService), typeof(CaseDetailService));
            container.Register(typeof(IInboundLpnGateway<>), typeof(InboundLpnGateway<>));
            container.Register(typeof(IInboundLpnService), typeof(InboundLpnService));
            container.Register(typeof(IInboundMessageService), typeof(InboundMessageService));
            container.Register(typeof(IInboundMessageGateway), typeof(InboundMessageGateway));
            container.Register(typeof(IInboundLpnGateway), typeof(InboundLpnGateway));
            container.Register(typeof(IInboundLpnRepository), typeof(InboundLpnRepository));

            container.Register(typeof(IMessageSourceGateway<>), typeof(MessageSourceGateway<>));
            container.Register(typeof(ISwmMessageSourceService), typeof(SwmMessageSourceService));
            container.Register(typeof(IMessageSourceUnitOfWork), typeof(MessageSourceUnitOfWork));

            container.Register(typeof(ITransitionalInventoryService), typeof(TransitionalInventoryService));
            container.Register(typeof(ITransitionalInventoryGateway), typeof(TransitionalInventoryGateway));
            container.Register(typeof(ITransitionalInventoryUnitOfWork), typeof(TransitionalInventoryUnitOfWork));

            container.Register(typeof(INextUpCounterService), typeof(NextUpCounterService));
            container.Register(typeof(INextUpCounterGateway), typeof(NextUpCounterGateway));
            container.Register(typeof(INextUpCounterUnitOfWork), typeof(NextUpCounterUnitOfWork));

            container.Register(typeof(ILocationUnitOfWork), typeof(LocationUnitOfWork));
            container.Register(typeof(IPickLocationDetailsService), typeof(PickLocationDetailsService));
            container.Register(typeof(ILocationHeaderService), typeof(LocationHeaderService));
            container.Register(typeof(IPickLocationDetailsExtenstionService), typeof(PickLocationDetailsExtenstionService));
            container.Register(typeof(ILocationGateway<>), typeof(LocationGateway<>));

            container.Register(typeof(ITaskGateway), typeof(TaskGateway));
            container.Register(typeof(ITaskDetailUnitOfWork), typeof(TaskDetailUnitOfWork));
            container.Register(typeof(ITaskDetailService), typeof(TaskDetailService));
            container.Register(typeof(ITaskHeaderService), typeof(TaskHeaderService));
            container.Register(typeof(ITaskDetailGateway<>), typeof(TaskDetailRepository<>));

            container.Register(typeof(IPixTransactionService), typeof(PixTransactionService));
            container.Register(typeof(IPixTransactionGateway), typeof(PixTransactionGateway));
            container.Register(typeof(IPixTransactionUnitOfWork), typeof(PixTransactionUnitOfWork));

            container.Register(typeof(ISkuInventoryService), typeof(SkuInventoryService));
            container.Register(typeof(ISkuInventoryGateway), typeof(SkuInventoryGateway));
            container.Register(typeof(ISkuInventoryUnitOfWork), typeof(SkuInventoryUnitOfWork));

            container.Register(typeof(ICartonHeaderService), typeof(CartonHeaderService));
            container.Register(typeof(ICartonDetailService), typeof(CartonDetailService));
            container.Register(typeof(IPickTicketHeaderService), typeof(PickTicketHeaderService));
            container.Register(typeof(IPickTicketDetailService), typeof(PickTicketDetailService));
            container.Register(typeof(ICartonUnitOfWork), typeof(CartonUnitOfWork));
            container.Register(typeof(ICartonGateway<>), typeof(CartonGateway<>));

            container.Register(typeof(IDockDoorMasterService), typeof(DockDoorMasterService));
            container.Register(typeof(IDockDoorUnitOfWork), typeof(DockDoorUnitOfWork));
            container.Register(typeof(IDockDoorGateway<>), typeof(DockDoorGateway<>));

            container.Register(typeof(IMessageToSortViewService), typeof(MessageToSortViewService));
            container.Register(typeof(IMessageUnitOfWork), typeof(MessageUnitOfWork));
            container.Register(typeof(IMessageGateway<>), typeof(MessageGateway<>));

            container.Register(typeof(IAllocationInventoryDetailService<>), typeof(AllocationInventoryDetailService));
            container.Register(typeof(IAllocationInventoryGateway<>), typeof(AllocationInventoryGateway<>));
            container.Register(typeof(IAllocationInventoryUnitOfWork), typeof(AllocationInventoryUnitOfWork));

            container.Register(typeof(ICanGenerateLabel), typeof(LabelGenerator));
        }
    }
}