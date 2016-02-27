using NLog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAdmin.Logging
{
    /*
     * 
     * ¡¡¡¡¡SOLO ENTRA EN ACCIÓN EN MODO ReleaseINTERCEPTOR!!!!!
     *  VER CLASE TrainingModelConfiguration.cs
     * 
     * The following are the allowed log levels (in descending order):
        • Off 
        • Fatal 
        • Error 
        • Warn 
        • Info 
        • Debug 
        • Trace 
     */
    public class NLogCommandInterceptor : IDbCommandInterceptor
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();

        public void NonQueryExecuting(
            DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            _stopwatch.Restart();
            LogIfNonAsync(command, interceptionContext);
            LogIfAdHoc(command);
        }

        public void NonQueryExecuted(
            DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            WriteLogEntry(command, _stopwatch.ElapsedMilliseconds);
            LogIfError(command, interceptionContext);
        }

        public void ReaderExecuting(
            DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            _stopwatch.Restart();
            LogIfNonAsync(command, interceptionContext);
            LogIfAdHoc(command);
        }

        public void ReaderExecuted(
            DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            WriteLogEntry(command, _stopwatch.ElapsedMilliseconds);
            LogIfError(command, interceptionContext);
        }

        public void ScalarExecuting(
            DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            _stopwatch.Restart();
            LogIfNonAsync(command, interceptionContext);
            LogIfAdHoc(command);
        }

        public void ScalarExecuted(
            DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            WriteLogEntry(command, _stopwatch.ElapsedMilliseconds);
            LogIfError(command, interceptionContext);           
        }

        private void LogIfNonAsync<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            if (!interceptionContext.IsAsync)
            {
                Logger.Warn("Non-async command used: {0}", command.CommandText);
            }
        }

        private void LogIfAdHoc(
           DbCommand command)
        {
            if (command.CommandType == System.Data.CommandType.Text)
            {
                Logger.Warn("Query adhoc detectada: {0}", command.CommandText);
            }
        }


        private void LogIfError<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            if (interceptionContext.Exception != null)
            {
                Logger.Error("Command {0} failed with exception {1}",
                    command.CommandText, interceptionContext.Exception);
            }
        }

        private void WriteLogEntry(DbCommand command,long elapsedMs)
        {
            string message = String.Format("{0}Tipo de comando: {1}{0}Tiempo de ejecución: {2}ms{0}Comando: {3}{0}", Environment.NewLine, command.CommandType.ToString(), elapsedMs.ToString(), command.CommandText);
            string messageTabulado = String.Format("{1}{0}{2}{0}{3}{0}", "|", command.CommandType.ToString(), elapsedMs.ToString(), command.CommandText.Replace("\r\n", ""));
            Logger.Info(message);            
            Logger.Trace(messageTabulado);
        }
    }
}
