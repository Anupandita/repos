using System;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Http;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Sfc.Core.Aop.WebApi.Logging;
using Sfc.Core.Cache.Contracts;
using Sfc.Core.Cache.InMemory;
using Sfc.Core.OnPrem.Pagination;
using Sfc.Core.OnPrem.Security.Contracts.Extensions;
using Sfc.Wms.App.App.AutoMapper;
using Sfc.Wms.Data.Context;
using Sfc.Wms.Data.Entities;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Framework.Interceptor.App.interceptors;
using Sfc.Wms.Framework.MessageLogger.App.Services;
using Sfc.Wms.Framework.MessageMaster.App.Services;
using Sfc.Wms.Framework.Security.Rbac.AutoMapper;
using Sfc.Wms.Inbound.InboundLpn.App.Validators;
using Sfc.Wms.Interfaces.Asrs.App.Mappers;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Interfaces;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

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
                     cfg.AddExpressionMapping();
                     SfcRbacMapper.CreateMaps(cfg);
                     PrinterValuesMapper.CreateMaps(cfg);
                     SfcAsrsMapper.CreateMaps(cfg);
                     cfg.CreateMap<CaseLock, CaseLockDto>().ReverseMap();
                     cfg.CreateMap<CaseDetail, CaseDetailDto>().ReverseMap();
                     cfg.CreateMap<CaseComment, CaseCommentDto>().ReverseMap();
                     cfg.CreateMap<CaseHeader, CaseHeaderDto>().ReverseMap();
                     cfg.CreateMap<CaseHeader, LpnHeaderUpdateDto>(MemberList.None).ReverseMap();
                     cfg.CreateMap<LpnParameterDto, PageOptions>(MemberList.None).ReverseMap();
                     cfg.CreateMap<PageOptions, LpnSearchResultsDto>(MemberList.None).ReverseMap();
                 }));
#if DEBUG
                // mapper.DefaultContext.ConfigurationProvider.AssertConfigurationIsValid();
#endif
                 return mapper;
             });

            container.Register(() => new ShamrockContext(ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString),
                Lifestyle.Scoped);
            container.Register(() => ConfigurationManager.AppSettings["db:encryptionKey"].ToSecureString(), Lifestyle.Singleton);
            container.Register<ISfcCache>(() => new SfcInMemoryCache(MemoryCache.Default), Lifestyle.Scoped);
            container.Register<IMappingFixture>(() => new MappingFixture(), Lifestyle.Singleton);
           container.Register<LpnParameterValidator>(Lifestyle.Scoped);

            container.Options.AllowOverridingRegistrations = true;
            container.Register<SfcLogger>(Lifestyle.Scoped);
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
                                                     && !reg.implementation.IsGenericTypeDefinition)
                    {
                        container.Register(reg.service, reg.implementation, Lifestyle.Scoped);
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
                        container.Register(reg.service.GetGenericTypeDefinition(),
                            reg.implementation.GetGenericTypeDefinition(), Lifestyle.Scoped);

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