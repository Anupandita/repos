using Sfc.Wms.Asrs.Dematic.Repository.Gateways;
using System;
using System.Linq.Expressions;
using Sfc.Wms.Data.Entities;

namespace Sfc.Wms.Asrs.Api.Utilities
{
    public class ExpressionBuilder
    {
        public static Expression<Func<T, bool>> GetPredicateByKey<T>(string prc, long msgKey)
            where T : DematicBaseEntity
        {
            return el => el.Process == prc && el.MessageKey == msgKey;
        }

        public static Expression<Func<T, bool>> GetPredicateByStatus<T>(string status) where T : DematicBaseEntity
        {
            return el => el.Status == status;
        }
    }
}