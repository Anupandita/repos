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
using Sfc.Wms.Configuration.DockDoor.Contracts.Interfaces;
using Sfc.Wms.Configuration.DockDoor.Repository.Gateway;
using Sfc.Wms.Configuration.DockDoor.Repository.Interfaces;
using Sfc.Wms.Configuration.ItemMaster.Contracts.Interface;
using Sfc.Wms.Configuration.ItemMaster.Repository.Gateways;
using Sfc.Wms.Configuration.ItemMaster.Repository.Interfaces;
using Sfc.Wms.Configuration.NextUpCounter.Contracts.Interfaces;
using Sfc.Wms.Configuration.NextUpCounter.Repository.Gateways;
using Sfc.Wms.Configuration.NextUpCounter.Repository.Interfaces;
using Sfc.Wms.Configuration.Security.Rbac.Repository.Context;
using Sfc.Wms.Configuration.Security.Rbac.Repository.Gateways;
using Sfc.Wms.Configuration.Security.Rbac.Repository.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Contracts.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Repository.Context;
using Sfc.Wms.Configuration.SystemCode.Repository.Gateways;
using Sfc.Wms.Configuration.SystemCode.Repository.Interfaces;
using Sfc.Wms.Data.Context;
using Sfc.Wms.Data.Interfaces;
using Sfc.Wms.Foundation.AllocationInventory.App.Interfaces;
using Sfc.Wms.Foundation.AllocationInventory.App.Services;
using Sfc.Wms.Foundation.AllocationInventory.App.UnitOfWork;
using Sfc.Wms.Foundation.AllocationInventory.Contracts.Interfaces;
using Sfc.Wms.Foundation.AllocationInventory.Repository.Gateway;
using Sfc.Wms.Foundation.AllocationInventory.Repository.Interfaces;
using Sfc.Wms.Foundation.Carton.Contracts.Interfaces;
using Sfc.Wms.Foundation.Carton.Repository.Gateway;
using Sfc.Wms.Foundation.Carton.Repository.Interfaces;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Interfaces;
using Sfc.Wms.Foundation.InboundLpn.Repository.Context;
using Sfc.Wms.Foundation.InboundLpn.Repository.Gateways;
using Sfc.Wms.Foundation.InboundLpn.Repository.Interfaces;
using Sfc.Wms.Foundation.InboundLpn.Repository.LocationDataRepository;
using Sfc.Wms.Foundation.Location.App.Interfaces;
using Sfc.Wms.Foundation.Location.App.Services;
using Sfc.Wms.Foundation.Location.App.UnitOfWork;
using Sfc.Wms.Foundation.Location.Contracts.Interfaces;
using Sfc.Wms.Foundation.Location.Repository.Gateway;
using Sfc.Wms.Foundation.Location.Repository.Interfaces;
using Sfc.Wms.Foundation.Message.Contracts.Interfaces;
using Sfc.Wms.Foundation.Message.Repository.Gateway;
using Sfc.Wms.Foundation.Message.Repository.Interfaces;
using Sfc.Wms.Foundation.MessageSource.Contracts.Interfaces;
using Sfc.Wms.Foundation.MessageSource.Repository.Gateways;
using Sfc.Wms.Foundation.MessageSource.Repository.Interfaces;
using Sfc.Wms.Foundation.PixTransaction.Contracts.Interfaces;
using Sfc.Wms.Foundation.PixTransaction.Repository.Gateway;
using Sfc.Wms.Foundation.PixTransaction.Repository.Interfaces;
using Sfc.Wms.Foundation.SkuInventory.Contracts.Interfaces;
using Sfc.Wms.Foundation.SkuInventory.Repository.Gateway;
using Sfc.Wms.Foundation.SkuInventory.Repository.Interfaces;
using Sfc.Wms.Foundation.Tasks.Contracts.Interfaces;
using Sfc.Wms.Foundation.Tasks.Repository.Gateways;
using Sfc.Wms.Foundation.Tasks.Repository.Interfaces;
using Sfc.Wms.Foundation.TransitionalInventory.Contracts.Interfaces;
using Sfc.Wms.Foundation.TransitionalInventory.Repository.Gateways;
using Sfc.Wms.Foundation.TransitionalInventory.Repository.Interfaces;
using Sfc.Wms.Framework.DockDoor.App.Interfaces;
using Sfc.Wms.Framework.DockDoor.App.Services;
using Sfc.Wms.Framework.DockDoor.App.UnitOfWork;
using Sfc.Wms.Framework.ItemMaster.App.Interfaces;
using Sfc.Wms.Framework.ItemMaster.App.Services;
using Sfc.Wms.Framework.ItemMaster.App.UnitOfWork;
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
using Sfc.Wms.Framework.SkuInventory.App.Interfaces;
using Sfc.Wms.Framework.SkuInventory.App.Services;
using Sfc.Wms.Framework.SkuInventory.App.UnitOfWork;
using Sfc.Wms.Framework.SystemCode.App.Interfaces;
using Sfc.Wms.Framework.SystemCode.App.Services;
using Sfc.Wms.Framework.SystemCode.App.UnitOfWork;
using Sfc.Wms.Inbound.InboundLpn.App.Interfaces;
using Sfc.Wms.Inbound.InboundLpn.App.Services;
using Sfc.Wms.Inbound.InboundLpn.App.UnitOfWork;
using Sfc.Wms.Inbound.InboundLpn.App.Validators;
using Sfc.Wms.Inbound.Tasks.App.Interfaces;
using Sfc.Wms.Inbound.Tasks.App.Services;
using Sfc.Wms.Inbound.Tasks.App.UnitOfWork;
using Sfc.Wms.Inbound.TransitionalInventory.App.Interfaces;
using Sfc.Wms.Inbound.TransitionalInventory.App.Services;
using Sfc.Wms.Inbound.TransitionalInventory.App.UnitOfWork;
using Sfc.Wms.Interfaces.Asrs.App.Interfaces;
using Sfc.Wms.Interfaces.Asrs.App.Mappers;
using Sfc.Wms.Interfaces.Asrs.App.Services;
using Sfc.Wms.Interfaces.Asrs.App.UnitOfWork;
using Sfc.Wms.Interfaces.Asrs.Contracts.Interfaces;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Interfaces;
using Sfc.Wms.Interfaces.Asrs.Dematic.Repository.Gateways;
using Sfc.Wms.Interfaces.Asrs.Dematic.Repository.Interfaces;
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
using Sfc.Wms.Outbound.Carton.App.Interfaces;
using Sfc.Wms.Outbound.Carton.App.Services;
using Sfc.Wms.Outbound.Carton.App.UnitOfWork;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System;
using System.Configuration;
using System.Reflection;
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
                     cfg.AddExpressionMapping();
                     SfcRbacMapper.CreateMaps(cfg);
                     PrinterValuesMapper.CreateMaps(cfg);
                     SfcAsrsMapper.CreateMaps(cfg);
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
            container.Register<IRbacGateway, RbacGateway>(Lifestyle.Scoped);
            container.Register<IUserRbacUnitOfWork, UserRbacUnitOfWork>(Lifestyle.Scoped);
            container.Register<IUserRbacService, UserRbacService>(Lifestyle.Scoped);
            container.Register<IUserAuthenticationGateway, UserAuthenticationGateway>(Lifestyle.Scoped);
            container.Register<ISfcCache>(() => new SfcInMemoryCache(MemoryCache.Default), Lifestyle.Scoped);

            container.Register(typeof(ISystemCodeService), typeof(SystemCodeService), Lifestyle.Scoped);
            container.Register(typeof(ISystemCodeRepository), typeof(SystemCodeRepository), Lifestyle.Scoped);
            container.Register(typeof(ISystemCodeUnitOfWork), typeof(SystemCodeUnitOfWork), Lifestyle.Scoped);
            container.Register(typeof(ISystemCodeRepositoryGateway), typeof(SystemCodeRepositoryGateway), Lifestyle.Scoped);
            container.Register(typeof(IRbacService), typeof(RbacService), Lifestyle.Scoped);

            container.Register<IMappingFixture>(() => new MappingFixture(), Lifestyle.Singleton);
            container.Register(typeof(IGenericRepository<>), typeof(GenericRepository<>), Lifestyle.Scoped);

            container.Register(typeof(IAsrsUnitOfWork), typeof(AsrsUnitOfWork), Lifestyle.Scoped);
            container.Register(typeof(IDematicGateway<>), typeof(IDematicGateway<>).Assembly, Lifestyle.Scoped);
            container.Register(typeof(IGenericRepository<>), typeof(GenericRepository<>).Assembly, Lifestyle.Scoped);
            container.Register(typeof(IShamrockGateway<>), typeof(IShamrockGateway<>).Assembly, Lifestyle.Scoped);
            container.Register(typeof(IEmsToWmsService), typeof(EmsToWmsService), Lifestyle.Scoped);
            container.Register(typeof(IWmsToEmsService), typeof(WmsToEmsService), Lifestyle.Scoped);
            container.Register(typeof(IDematicGateway<>), typeof(DematicGateway<>), Lifestyle.Scoped);
            container.Register(typeof(ISwmFromMheService), typeof(SwmFromMheService), Lifestyle.Scoped);
            container.Register(typeof(ISwmToMheService), typeof(SwmToMheService), Lifestyle.Scoped);
            container.Register(typeof(IShamrockGateway<>), typeof(ShamrockRepository<>), Lifestyle.Scoped);
            container.Register(typeof(IWmsToEmsMessageProcessorService), typeof(WmsToEmsMessageProcessorService), Lifestyle.Scoped);
            container.Register(typeof(IEmsToWmsMessageProcessorService), typeof(EmsToWmsMessageProcessorService), Lifestyle.Scoped);

            container.Register(typeof(IBuildMessage), typeof(MainMessageBuilder), Lifestyle.Scoped);
            container.Register(typeof(IParseMessage), typeof(MainParser), Lifestyle.Scoped);
            container.Register(typeof(ITranslateDematicMessage), typeof(DematicTranslator), Lifestyle.Scoped);
            container.Register(typeof(IWmsToEmsTranslator), typeof(WmsToEmsTranslator), Lifestyle.Scoped);
            container.Register(typeof(IEmsToWmsTranslator), typeof(EmsToWmsTranslator), Lifestyle.Scoped);

            container.Register(typeof(IInboundLpnUnitOfWork), typeof(InboundLpnUnitOfWork), Lifestyle.Scoped);
            container.Register(typeof(ICaseHeaderService), typeof(CaseHeaderService), Lifestyle.Scoped);
            container.Register(typeof(ICaseDetailService), typeof(CaseDetailService), Lifestyle.Scoped);
            container.Register(typeof(IInboundLpnGenericGateway<>), typeof(InboundLpnGenericGateway<>), Lifestyle.Scoped);
            container.Register(typeof(IInboundLpnService), typeof(InboundLpnService), Lifestyle.Scoped);
            container.Register(typeof(IInboundMessageService), typeof(InboundMessageService), Lifestyle.Scoped);
            container.Register(typeof(IInboundMessageGateway), typeof(InboundMessageGateway), Lifestyle.Scoped);
            container.Register(typeof(IInboundLpnGateway), typeof(InboundLpnGateway), Lifestyle.Scoped);
            container.Register(typeof(IInboundLpnRepository), typeof(InboundLpnRepository), Lifestyle.Scoped);

            container.Register(typeof(IMessageSourceGateway<>), typeof(MessageSourceGateway<>), Lifestyle.Scoped);
            container.Register(typeof(ISwmMessageSourceService), typeof(SwmMessageSourceService), Lifestyle.Scoped);
            container.Register(typeof(IMessageSourceUnitOfWork), typeof(MessageSourceUnitOfWork), Lifestyle.Scoped);

            container.Register(typeof(ITransitionalInventoryService), typeof(TransitionalInventoryService), Lifestyle.Scoped);
            container.Register(typeof(ITransitionalInventoryGateway), typeof(TransitionalInventoryGateway), Lifestyle.Scoped);
            container.Register(typeof(ITransitionalInventoryUnitOfWork), typeof(TransitionalInventoryUnitOfWork), Lifestyle.Scoped);

            container.Register(typeof(INextUpCounterService), typeof(NextUpCounterService), Lifestyle.Scoped);
            container.Register(typeof(INextUpCounterGateway), typeof(NextUpCounterGateway), Lifestyle.Scoped);
            container.Register(typeof(INextUpCounterUnitOfWork), typeof(NextUpCounterUnitOfWork), Lifestyle.Scoped);

            container.Register(typeof(ILocationUnitOfWork), typeof(LocationUnitOfWork), Lifestyle.Scoped);
            container.Register(typeof(IPickLocationDetailsService), typeof(PickLocationDetailsService), Lifestyle.Scoped);
            container.Register(typeof(ILocationHeaderService), typeof(LocationHeaderService), Lifestyle.Scoped);
            container.Register(typeof(IPickLocationDetailsExtenstionService), typeof(PickLocationDetailsExtenstionService), Lifestyle.Scoped);
            container.Register(typeof(ILocationGateway<>), typeof(LocationGateway<>), Lifestyle.Scoped);

            container.Register(typeof(ITaskGateway), typeof(TaskGateway), Lifestyle.Scoped);
            container.Register(typeof(ITaskDetailUnitOfWork), typeof(TaskDetailUnitOfWork), Lifestyle.Scoped);
            container.Register(typeof(ITaskDetailService), typeof(TaskDetailService), Lifestyle.Scoped);
            container.Register(typeof(ITaskHeaderService), typeof(TaskHeaderService), Lifestyle.Scoped);
            container.Register(typeof(ITaskDetailGateway<>), typeof(TaskDetailRepository<>), Lifestyle.Scoped);

            container.Register(typeof(IPixTransactionService), typeof(PixTransactionService), Lifestyle.Scoped);
            container.Register(typeof(IPixTransactionGateway), typeof(PixTransactionGateway), Lifestyle.Scoped);
            container.Register(typeof(IPixTransactionUnitOfWork), typeof(PixTransactionUnitOfWork), Lifestyle.Scoped);

            container.Register(typeof(ISkuInventoryService), typeof(SkuInventoryService), Lifestyle.Scoped);
            container.Register(typeof(ISkuInventoryGateway), typeof(SkuInventoryGateway), Lifestyle.Scoped);
            container.Register(typeof(ISkuInventoryUnitOfWork), typeof(SkuInventoryUnitOfWork), Lifestyle.Scoped);

            container.Register(typeof(ICartonHeaderService), typeof(CartonHeaderService), Lifestyle.Scoped);
            container.Register(typeof(ICartonDetailService), typeof(CartonDetailService), Lifestyle.Scoped);
            container.Register(typeof(IPickTicketHeaderService), typeof(PickTicketHeaderService), Lifestyle.Scoped);
            container.Register(typeof(IPickTicketDetailService), typeof(PickTicketDetailService), Lifestyle.Scoped);
            container.Register(typeof(ICartonUnitOfWork), typeof(CartonUnitOfWork), Lifestyle.Scoped);
            container.Register(typeof(ICartonGateway<>), typeof(CartonGateway<>), Lifestyle.Scoped);

            container.Register(typeof(IDockDoorMasterService), typeof(DockDoorMasterService), Lifestyle.Scoped);
            container.Register(typeof(IDockDoorUnitOfWork), typeof(DockDoorUnitOfWork), Lifestyle.Scoped);
            container.Register(typeof(IDockDoorGateway<>), typeof(DockDoorGateway<>), Lifestyle.Scoped);

            container.Register(typeof(IMessageToSortViewService), typeof(MessageToSortViewService), Lifestyle.Scoped);
            container.Register(typeof(IMessageUnitOfWork), typeof(MessageUnitOfWork), Lifestyle.Scoped);
            container.Register(typeof(IMessageGateway<>), typeof(MessageGateway<>), Lifestyle.Scoped);

            container.Register(typeof(IAllocationInventoryDetailService<>), typeof(AllocationInventoryDetailService), Lifestyle.Scoped);
            container.Register(typeof(IAllocationInventoryGateway<>), typeof(AllocationInventoryGateway<>), Lifestyle.Scoped);
            container.Register(typeof(IAllocationInventoryUnitOfWork), typeof(AllocationInventoryUnitOfWork), Lifestyle.Scoped);

            container.Register(typeof(ICanGenerateLabel), typeof(LabelGenerator), Lifestyle.Scoped);
            container.Register(typeof(IFindLpnService), typeof(FindLpnService), Lifestyle.Scoped);
            container.Register(typeof(ILpnHistoryService), typeof(LpnHistoryService), Lifestyle.Scoped);

            container.Register(typeof(IItemMasterService), typeof(ItemMasterService), Lifestyle.Scoped);
            container.Register(typeof(IItemMasterGateway<>), typeof(ItemMasterGateway<>), Lifestyle.Scoped);
            container.Register(typeof(IItemMasterUnitOfWork), typeof(ItemMasterUnitOfWork), Lifestyle.Scoped);

            container.Register<UserRepository>(Lifestyle.Scoped);
            container.Register<LocationData>(Lifestyle.Scoped);
            container.Register<LpnParameterValidator>(Lifestyle.Scoped);

        }
    }
}