using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Sfc.Core.OnPrem.AutoMapping.Initialize;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Http;
using Oracle.ManagedDataAccess.Client;
using Sfc.Core.Cache.Contracts;
using Sfc.Core.Cache.InMemory;
using Sfc.Core.OnPrem.UnitOfWork.Contracts.UoW.Interfaces;
using Sfc.Wms.Data.Domain;
using Sfc.Wms.Data.Domain.Interfaces;
using Sfc.Wms.Framework.SystemCode.App.UoW.Services;
using Sfc.Wms.Configuration.SystemCode.Contracts.UoW.Interfaces;
using Sfc.Wms.Data.UoW;

namespace Sfc.Wms.App.Api
{
    public static class DependencyConfigUoW
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<IUoWFactory, DbUoWFactory>(Lifestyle.Scoped);
            container.Register<IEntityMapper, EntityMapper>(Lifestyle.Singleton);

            container.Register<ISystemCodeService, SystemCodeService>(Lifestyle.Scoped);
            container.Register<ISfcCache>(() => new SfcInMemoryCache(MemoryCache.Default), Lifestyle.Singleton);

            var connectionString = ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString;
            container.Register<DbConnection>(() => new OracleConnection(connectionString),
            Lifestyle.Scoped);

            container.Options.AllowOverridingRegistrations = true;
        }
    }
}