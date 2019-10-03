using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sfc.Wms.App.Api.Contracts.Constants;

namespace Wms.App.Api.Nuget.Builders
{
    class QueryStringBuilder
    {
        public static string BuildQuery(string field, dynamic value, string query, bool firstCondition)
        {
            if ((value != null && value.GetType() == typeof(System.String)) && !System.String.IsNullOrEmpty(value))
            {
                query = $"{query}{(firstCondition ? "" : Routes.Paths.QueryParamAnd)}{field}{value}";
                return query;
            }
            if ((value != null && value.GetType() == typeof(System.Int32)) && value != 0)
            {
                query = $"{query}{(firstCondition ? "" : Routes.Paths.QueryParamAnd)}{field}{value}";
                return query;
            }
            return query;
        }
    }
}
