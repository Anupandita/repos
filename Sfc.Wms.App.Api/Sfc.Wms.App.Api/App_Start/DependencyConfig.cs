using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Sfc.Core.Cache.Contracts;
using Sfc.Core.Cache.InMemory;
using Sfc.Core.OnPrem.Security.Contracts.Extensions;
using Sfc.Wms.App.App.AutoMapper;
using Sfc.Wms.Configuration.Security.Rbac.Repository.Context;
using Sfc.Wms.Data.Context;
using Sfc.Wms.Foundation.InboundLpn.Repository.LocationDataRepository;
using Sfc.Wms.Framework.Security.Rbac.AutoMapper;
using Sfc.Wms.Inbound.InboundLpn.App.Validators;
using Sfc.Wms.Interfaces.Asrs.App.Mappers;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System;
using System.Configuration;
using System.Linq;
using System.Linq.Dynamic;
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

            container.Register(() => new ShamrockContext(ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString), 
                Lifestyle.Singleton);
            container.Register(() => ConfigurationManager.AppSettings["db:encryptionKey"].ToSecureString(), Lifestyle.Singleton);
            container.Register<ISfcCache>(() => new SfcInMemoryCache(MemoryCache.Default), Lifestyle.Scoped);
            container.Register<IMappingFixture>(() => new MappingFixture(), Lifestyle.Singleton);
            container.Register<UserRepository>(Lifestyle.Scoped);
            container.Register<LocationData>(Lifestyle.Scoped);
            container.Register<LpnParameterValidator>(Lifestyle.Scoped);

            container.Options.AllowOverridingRegistrations = true;

            var types = AppDomain.CurrentDomain.GetAssemblies().Where(e => e.FullName.StartsWith("Sfc"));

            foreach (var assemblyInfo in types)
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
                        && !reg.implementation.IsGenericTypeDefinition)
                        container.Register(reg.service, reg.implementation, Lifestyle.Scoped);
                    else if (reg.implementation.IsGenericTypeDefinition)
                        container.Register(reg.service.GetGenericTypeDefinition(), reg.implementation.GetGenericTypeDefinition(), Lifestyle.Scoped);
                }
            }
        }
    }
}