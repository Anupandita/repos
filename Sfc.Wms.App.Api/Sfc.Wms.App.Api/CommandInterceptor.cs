using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Text;

namespace Sfc.Wms.App.Api
{
    public class CommandInterceptor : IDbCommandInterceptor
    {
        private static readonly ConcurrentDictionary<DbCommand, DateTime> MStartTime = new ConcurrentDictionary<DbCommand, DateTime>();
        private readonly QueryLogger _queryLogger;

        public CommandInterceptor(QueryLogger queryLogger)
        {
            _queryLogger = queryLogger;
        }

        void IDbCommandInterceptor.NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            Log(command, interceptionContext);
        }

        void IDbCommandInterceptor.NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            OnStart(command);
        }

        void IDbCommandInterceptor.ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            Log(command, interceptionContext);
        }

        void IDbCommandInterceptor.ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            OnStart(command);
        }

        void IDbCommandInterceptor.ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            Log(command, interceptionContext);
        }

        void IDbCommandInterceptor.ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            OnStart(command);
        }

        private void OnStart(DbCommand command)
        {
            MStartTime.TryAdd(command, DateTime.Now);
        }

        public void Log<T>(DbCommand command, DbCommandInterceptionContext<T> interceptionContext)
        {
            TimeSpan duration;

            MStartTime.TryRemove(command, out var startTime);
            if (startTime != default(DateTime))
            {
                duration = DateTime.Now - startTime;
            }
            else
            {
                duration = TimeSpan.Zero;
            }

            var parameters = new StringBuilder();
            foreach (DbParameter param in command.Parameters)
            {
                parameters.AppendLine(param.ParameterName + " " + param.DbType + " = " + param.Value);
            }

            //if (duration.Milliseconds < 1000)
            //    return;

            var message = interceptionContext.Exception == null
                ? $"\r\nDatabase call took {duration.TotalMilliseconds} ms. \r\nCommand:\r\n{parameters + command.CommandText}\r\n"
                : $"\r\nEF Database call failed after {duration.TotalMilliseconds} ms. \r\nCommand:\r\n{parameters + command.CommandText}\r\nError:{interceptionContext.Exception} ";

            _queryLogger.Write(message);
        }
    }
}