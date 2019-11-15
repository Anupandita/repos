using Sfc.Wms.App.Api.Contracts.Constants;

namespace Sfc.Wms.App.Api.Nuget.Builders
{
    class QueryStringBuilder
    {
        public static string BuildQuery(string field, dynamic value, string query, bool firstCondition)
        {
            if ((value != null && value is string) && !string.IsNullOrEmpty(value))
            {
                query = $"{query}{(firstCondition ? "" : Routes.Paths.QueryParamAnd)}{field}{value}";
                return query;
            }
            if ((value != null && value is int) && value != 0)
            {
                query = $"{query}{(firstCondition ? "" : Routes.Paths.QueryParamAnd)}{field}{value}";
                return query;
            }
            return query;
        }
    }
}
