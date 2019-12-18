using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.IO;
using System.Text;
using Serilog;

namespace Sfc.Wms.App.Api
{
    public class CommandInterceptor : IDbCommandInterceptor
    {
        private static readonly ConcurrentDictionary<DbCommand, DateTime> MStartTime = new ConcurrentDictionary<DbCommand, DateTime>();

        //private readonly QueryLogger _queryLogger;
        private readonly ILogger _logger;

        public CommandInterceptor()
        {
            //_queryLogger = queryLogger;
            _logger = new LoggerConfiguration().WriteTo
                .File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["QueryLogFileName"]),
                    rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger(); ;
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
            if (ConfigurationManager.AppSettings["EnableQueryLogging"] != "true") return;
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

            if (duration.Milliseconds < int.Parse(ConfigurationManager.AppSettings["QueriesToLogWithMinimumTime"]))
                return;

            var parameters = new StringBuilder();
            foreach (DbParameter param in command.Parameters)
            {
                parameters.AppendLine(param.ParameterName + " " + param.DbType + " = " + param.Value);
            }

            var message = interceptionContext.Exception == null
                ? $"\r\nDatabase call took {duration.TotalMilliseconds} ms. \r\nCommand:\r\n{parameters + command.CommandText}\r\n"
                : $"\r\nEF Database call failed after {duration.TotalMilliseconds} ms. \r\nCommand:\r\n{parameters + command.CommandText}\r\nError:{interceptionContext.Exception} ";
            _logger.Information(message);

            //_queryLogger.Write(message);
        }
    }
}