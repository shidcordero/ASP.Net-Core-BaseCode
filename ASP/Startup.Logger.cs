using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ASP
{
    public partial class Startup
    {
        private void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            var logingConfig = Configuration.GetSection("Serilog");
            var logDir = logingConfig.GetValue<string>("output");

            loggerFactory.AddConsole(logingConfig);
            loggerFactory.AddDebug();

            var logEventLevel = logingConfig.GetValue<Serilog.Events.LogEventLevel>("level");
            var serilogger = new LoggerConfiguration()
                .MinimumLevel.Is(logEventLevel)
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(logDir + "\\{Date}.txt")
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(le => le.Level == Serilog.Events.LogEventLevel.Debug)
                    .WriteTo.RollingFile(logDir + "\\{Date}_Debug.txt"))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(le => le.Level == Serilog.Events.LogEventLevel.Warning)
                    .WriteTo.RollingFile(logDir + "\\{Date}_Warning.txt"))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(le => le.Level == Serilog.Events.LogEventLevel.Error)
                    .WriteTo.RollingFile(logDir + "\\{Date}_Error.txt"))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(le => le.Level == Serilog.Events.LogEventLevel.Information)
                    .WriteTo.RollingFile(logDir + "\\{Date}_Information.txt"))
                .CreateLogger();

            loggerFactory.AddSerilog(serilogger);
        }
    }
}