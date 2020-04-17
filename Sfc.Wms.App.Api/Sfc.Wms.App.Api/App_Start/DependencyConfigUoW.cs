using Oracle.ManagedDataAccess.Client;
using Sfc.Core.OnPrem.Cache.InMemory;
using Sfc.Core.OnPrem.UnitOfWork.Contracts.UoW.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Contracts.UoW.Interfaces;
using Sfc.Wms.Data.Domain;
using Sfc.Wms.Data.Domain.Interfaces;
using Sfc.Wms.Data.UoW;
using Sfc.Wms.Foundation.Receiving.Contracts.UoW.Interfaces;
using Sfc.Wms.Framework.SystemCode.App.UoW.Services;
using Sfc.Wms.Inbound.Receiving.App.UoW.Services;
using SimpleInjector;
using System.Configuration;
using System.Data.Common;
using System.Runtime.Caching;

namespace Sfc.Wms.App.Api
{
    public static class DependencyConfigUoW
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<IUoWFactory, DbUoWFactory>(Lifestyle.Scoped);
            container.Register<IEntityMapper, EntityMapper>(Lifestyle.Singleton);

            container.Register<ISystemCodeService, SystemCodeService>(Lifestyle.Scoped);
            container.Register<ISfcInMemoryCache>(() => new SfcInMemoryCache(MemoryCache.Default), Lifestyle.Singleton);

            container.Register<IInboundReceivingService, InboundReceivingService>(Lifestyle.Transient);
            container.Register<IAsnDetailService, AsnDetailService>(Lifestyle.Transient);
            container.Register<IAsnHeaderService, AsnHeaderService>(Lifestyle.Transient);
            container.Register<IAsnLotTrackingService, AsnLotTrackingService>(Lifestyle.Transient);

            container.Register<IQuestionAnswerService, QuestionAnswerService>(Lifestyle.Transient);
            container.Register<IQuestionMasterService, QuestionMasterService>(Lifestyle.Transient);
            container.Register<IAsnCommentService, AsnCommentService>(Lifestyle.Transient);

            var connectionString = ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString;
            container.Register<DbConnection>(() => new OracleConnection(connectionString),
            Lifestyle.Scoped);

            container.Options.AllowOverridingRegistrations = true;
        }
    }
}