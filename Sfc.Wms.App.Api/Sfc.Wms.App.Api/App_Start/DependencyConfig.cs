using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Sfc.Core.Aop.WebApi.Interface;
using Sfc.Core.Aop.WebApi.Logging;
using Sfc.Core.Cache.Contracts;
using Sfc.Core.Cache.InMemory;
using Sfc.Core.OnPrem.Pagination;
using Sfc.Core.OnPrem.Security.Contracts.Extensions;
using Sfc.Wms.App.App.AutoMapper;
using Sfc.Wms.Configuration.VendorMasters.Contracts.Dtos;
using Sfc.Wms.Data.Context;
using Sfc.Wms.Data.Entities;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Wms.Framework.Corba.App.AutoMapper;
using Sfc.Wms.Framework.Interceptor.App.interceptors;
using Sfc.Wms.Framework.ItemMasters.App.AutoMapper;
using Sfc.Wms.Framework.MessageLogger.App.AutoMapper;
using Sfc.Wms.Framework.MessageLogger.App.Services;
using Sfc.Wms.Framework.MessageMaster.App.AutoMapper;
using Sfc.Wms.Framework.MessageMaster.App.Services;
using Sfc.Wms.Framework.Security.Rbac.AutoMapper;
using Sfc.Wms.Inbound.InboundLpn.App.Validators;
using Sfc.Wms.Interfaces.Asrs.App.Mappers;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Interfaces;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Http;
using Sfc.Core.OnPrem.AutoMapping.Initialize;
using Sfc.Wms.Foundation.Receiving.Domain.AutoMapper;

namespace Sfc.Wms.App.Api
{
    public static class DependencyConfig
    {
        public static Container Register()
        {
            var container = new Container { Options = { DefaultScopedLifestyle = new AsyncScopedLifestyle() } };
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
                     cfg.Advanced.AllowAdditiveTypeMapCreation = true;
                     cfg.AddExpressionMapping();
                     SfcRbacMapper.CreateMaps(cfg);
                     PrinterValuesMapper.CreateMaps(cfg);
                     SfcAsrsMapper.CreateMaps(cfg);
                     SfcItemAttributeMapper.CreateMaps(cfg);
                     SfcCorbaMapper.CreateMaps(cfg);
                     SfcMessageMaster.CreateMaps(cfg);
                     SfcMessageLogger.CreateMaps(cfg);
                     SfcReceivingMapper.CreateMaps(cfg);
                     cfg.CreateMap<VendorMaster, VendorDetailDto>(MemberList.None).ReverseMap();
                     cfg.CreateMap<VendorMaster, VendorMasterDto>(MemberList.None).ReverseMap();

                     cfg.CreateMap<CaseLock, CaseLockDto>().ReverseMap();
                     cfg.CreateMap<CaseComment, CaseCommentDto>(MemberList.None).ReverseMap();
                     cfg.CreateMap<CaseHeader, LpnHeaderUpdateDto>(MemberList.None)
                         .ForMember(d => d.ExpireDate, s => s.MapFrom(e => e.ExpiryDate))
                         .ReverseMap();
                     cfg.CreateMap<LpnParameterDto, PageOptions>(MemberList.None).ReverseMap();
                     cfg.CreateMap<PageOptions, LpnSearchResultsDto>(MemberList.None).ReverseMap();
                     cfg.CreateMap<LocationHeaderDto, ContactLocationDto>(MemberList.None);

                     var assemblyList = AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic && p.FullName.StartsWith("Sfc.")).ToList();

                     cfg.AddMaps(assemblyList);
                     AutoProfilerConfigurator
                         .LoadMapsFromAssemblies(cfg, assemblyList);
                 }));
#if DEBUG
                 mapper.DefaultContext.ConfigurationProvider.AssertConfigurationIsValid();
#endif
                 return mapper;
             });
            DependencyConfigUoW.RegisterTypes(container);

            container.Register(() => new ShamrockContext(ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString),
                Lifestyle.Scoped);
            container.Register(() => ConfigurationManager.AppSettings["db:encryptionKey"].ToSecureString(), Lifestyle.Singleton);
            container.Register<ISfcCache>(() => new SfcInMemoryCache(MemoryCache.Default), Lifestyle.Scoped);
            container.Register<IMappingFixture>(() => new MappingFixture(), Lifestyle.Singleton);
            container.Register<LpnParameterValidator>(Lifestyle.Scoped);

            container.Options.AllowOverridingRegistrations = true;
            container.Register<SfcLogger>(Lifestyle.Singleton);
            container.Register<ISfcLoggerSerilogs, SfcLoggerSerilogs>(Lifestyle.Singleton);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(e => e.FullName.StartsWith("Sfc"));
            foreach (var assemblyInfo in assemblies)
            {
                var registrations = from type in assemblyInfo.GetExportedTypes()
                                    where type.Namespace != null && type.Namespace.StartsWith("Sfc") && type.IsClass
                                          && !type.IsAbstract && !type.IsInterface
                                    from service in type.GetInterfaces()
                                    select new { service, implementation = type };
                foreach (var reg in registrations)
                {
                    if (reg.service.FullName != null && reg.implementation.FullName != null
                                                     && reg.service.FullName.StartsWith("Sfc")
                                                     && !reg.implementation.FullName.Contains(
                                                         nameof(SfcInMemoryCache))
                                                     && !reg.implementation.FullName.Contains(
                                                         nameof(MonitoringInterceptor))
                                                     && !reg.implementation.IsGenericTypeDefinition
                                                     && !reg.service.FullName.Contains(nameof(SfcLoggerSerilogs)))
                    {
                        if (!reg.implementation.FullName.Contains(".UoW"))
                        {
                            container.Register(reg.service, reg.implementation, Lifestyle.Scoped);
                        }
                        if (reg.service.FullName.Contains(".Contracts") && !reg.implementation.FullName.Contains(nameof(MessageDetailService)) &&
                            !reg.implementation.FullName.Contains(nameof(MessageMasterService)) &&
                            !reg.implementation.FullName.Contains(nameof(MessageLogService)) &&
                            !reg.implementation.FullName.Contains("Aop"))
                        {
                            container.InterceptWith<MonitoringInterceptor>(type => type == reg.service);
                        }
                    }
                    else if (reg.implementation.IsGenericTypeDefinition)
                    {
                        if (reg.implementation.FullName != null && !reg.implementation.FullName.Contains(".UoW"))
                        {
                            container.Register(reg.service.GetGenericTypeDefinition(),
                                reg.implementation.GetGenericTypeDefinition(), Lifestyle.Scoped);
                        }

                        if (reg.service.FullName != null && reg.service.FullName.Contains(".Contracts") && reg.implementation.FullName != null &&
                                                             !reg.implementation.FullName.Contains(nameof(MessageDetailService)) &&
                                                              !reg.implementation.FullName.Contains(nameof(MessageMasterService)) &&
                                                              !reg.implementation.FullName.Contains(nameof(MessageLogService)) &&
                                                             !reg.implementation.FullName.Contains("Aop"))
                        {
                            container.InterceptWith<MonitoringInterceptor>(type =>
                                type == reg.service.GetGenericTypeDefinition());
                        }
                    }
                }
            }
        }
    }
}