using Oracle.ManagedDataAccess.Client;
using Sfc.Core.OnPrem.Cache.InMemory;
using Sfc.Core.OnPrem.UnitOfWork.Contracts.UoW.Interfaces;
using Sfc.Wms.Data.Domain;
using Sfc.Wms.Data.Domain.Interfaces;
using Sfc.Wms.Data.UoW;
using Sfc.Wms.Framework.Interceptor.App.UoW.interceptors;
using Sfc.Wms.Framework.MessageLogger.App.UoW.Services;
using Sfc.Wms.Framework.MessageMaster.App.UoW.Services;
using SimpleInjector;
using System;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Runtime.Caching;

namespace Sfc.Wms.App.Api
{
    public static class DependencyConfigUoW
    {
        public static void RegisterTypes(Container container)
        {
            container.Options.AllowOverridingRegistrations = true;

            var connectionString = ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString;
            container.Register<DbConnection>(() => new OracleConnection(connectionString), Lifestyle.Scoped);

            container.Register<IUoWFactory, DbUoWFactory>(Lifestyle.Scoped);
            container.Register<IEntityMapper, EntityMapper>(Lifestyle.Singleton);
            container.Register<ISfcInMemoryCache>(() => new SfcInMemoryCache(MemoryCache.Default), Lifestyle.Singleton);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(e => e.FullName.Contains(".App.UoW")).ToList();

            foreach (var assemblyInfo in assemblies)
            {
                var registrations = (from type in assemblyInfo.GetExportedTypes()
                                     where type.IsClass && !type.IsAbstract && !type.IsInterface
                                           && type.Namespace != null && type.Namespace.StartsWith("Sfc")
                                           && type.FullName != null 
                                            && !type.FullName.Contains(nameof(MonitoringInterceptorUoW))
                                     from service in type.GetInterfaces() where !service.FullName.Contains(typeof(ISfcService<>).Name)
                                     select new { service, implementation = type }).ToList();

                foreach (var reg in registrations)
                {
                    if (reg.service.FullName?.StartsWith("Sfc.") == true
                        && reg.implementation.Namespace?.Contains(".App.UoW") == true)
                    {
                        if (reg.service.IsGenericTypeDefinition)
                            container.Register(reg.service.GetGenericTypeDefinition(),
                                reg.implementation.GetGenericTypeDefinition(), Lifestyle.Scoped);
                        else
                            container.Register(reg.service, reg.implementation, Lifestyle.Scoped);
                    }

                    if (reg.service.FullName?.Contains(".Contracts.UoW") != true ||
                        reg.implementation.FullName == null ||
                        reg.implementation.FullName.Contains(nameof(MessageDetailService)) ||
                        reg.implementation.FullName.Contains(nameof(MessageMasterService)) ||
                        reg.implementation.FullName.Contains(nameof(MessageLogService)) ||
                        reg.implementation.FullName.Contains("Aop")) continue;

                    if (reg.implementation.IsGenericTypeDefinition)
                        container.InterceptWith<MonitoringInterceptorUoW>(type =>
                            type == reg.service.GetGenericTypeDefinition());
                    else
                        container.InterceptWith<MonitoringInterceptorUoW>(type => type == reg.service);
                }
            }

        }
    }
}