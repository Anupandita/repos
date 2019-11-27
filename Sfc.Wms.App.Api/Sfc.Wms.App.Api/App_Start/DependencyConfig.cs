using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Sfc.Core.Cache.Contracts;
using Sfc.Core.Cache.InMemory;
using Sfc.Core.OnPrem.Security.Contracts.Extensions;
using Sfc.Wms.App.App.AutoMapper;
using Sfc.Wms.Data.Context;
using Sfc.Wms.Foundation.InboundLpn.Repository.LocationDataRepository;
using Sfc.Wms.Interfaces.Asrs.App.Mappers;
using SimpleInjector.Lifestyles;
using System;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Http;
using Sfc.Core.Aop.WebApi.Logging;
using Sfc.Core.OnPrem.Security.Contracts.Interfaces;
using Sfc.Wms.Configuration.MessageLogger.Contracts.Interfaces;
using Sfc.Wms.Configuration.MessageMaster.Contracts.Interfaces;
using Sfc.Wms.Configuration.MessageTypes.Contracts.Interface;
using Sfc.Wms.Framework.Interceptor.App.interceptors;
using Sfc.Wms.Framework.MessageLogger.App.Services;
using Sfc.Wms.Framework.MessageMaster.App.Services;
using Sfc.Wms.Framework.MessageTypes.App.Services;
using Sfc.Wms.Framework.Security.Rbac.AutoMapper;
using Sfc.Wms.Inbound.InboundLpn.App.Validators;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Interfaces;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;


namespace Sfc.Wms.App.Api
{
    public static class DependencyConfig
    {
        public static Container Register()
        {
            var container = new Container {Options = {DefaultScopedLifestyle = new AsyncScopedLifestyle()}};
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

            container.Register(() => new ShamrockContext(ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString),
                Lifestyle.Scoped);
            container.Register(() => ConfigurationManager.AppSettings["db:encryptionKey"].ToSecureString(), Lifestyle.Singleton);
            container.Register<ISfcCache>(() => new SfcInMemoryCache(MemoryCache.Default), Lifestyle.Scoped);
            container.Register<IMappingFixture>(() => new MappingFixture(), Lifestyle.Singleton);
            container.Register<LocationData>(Lifestyle.Scoped);
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
                                                     && !reg.implementation.FullName.Contains(nameof(SfcInMemoryCache))
                                                     && !reg.implementation.FullName.Contains(
                                                         nameof(MonitoringInterceptor))
                                                     && !reg.implementation.IsGenericTypeDefinition)
                    {
                        container.Register(reg.service, reg.implementation, Lifestyle.Scoped);
                      //  container.InterceptWith<MonitoringInterceptor>(type => type == reg.service.BaseType);


                    }
                    else if (reg.implementation.IsGenericTypeDefinition)
                    {
                        container.Register(reg.service.GetGenericTypeDefinition(),
                            reg.implementation.GetGenericTypeDefinition(), Lifestyle.Scoped);

                        //container.InterceptWith<MonitoringInterceptor>(type =>
                         //   type == reg.service.GetGenericTypeDefinition().BaseType);
                    }

                }
            }
          //  container.Register(typeof(IMessageTypeService),typeof(MessageTypeService),Lifestyle.Scoped);
            container.Register(typeof(IMessageDetailService), typeof(MessageDetailService), Lifestyle.Scoped);

            container.Register(typeof(IMessageMasterService), typeof(MessageMasterService), Lifestyle.Scoped);
            container.Register(typeof(IMessageLogService), typeof(MessageLogService), Lifestyle.Scoped);

            container.InterceptWith<MonitoringInterceptor>(type => type == typeof(IUserRbacService).BaseType);

        }
    }
}