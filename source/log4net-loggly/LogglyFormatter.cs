using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ServiceStack.Text;
using log4net.Core;

namespace log4net.loggly
{
	public class LogglyFormatter : ILogglyFormatter
	{
        public LogglyFormatter()
        {
            JsConfig.ExcludeTypeInfo = true;
        }

		public virtual void AppendAdditionalLoggingInformation(ILogglyAppenderConfig config, LoggingEvent loggingEvent)
		{
		}

		public virtual string ToJson(LoggingEvent loggingEvent)
		{
			return PreParse(loggingEvent).ToJson();
		}

		public virtual string ToJson(IEnumerable<LoggingEvent> loggingEvents)
		{
			return loggingEvents.Select(PreParse).ToJson();
		}

		private object PreParse(LoggingEvent loggingEvent)
		{
			var exceptionString = loggingEvent.GetExceptionString();
            var message = loggingEvent.RenderedMessage;

            //ensure empty strings aren't included in the json output. Pass null instead.
			return new
			       	{
			       		level = loggingEvent.Level.DisplayName,
			       		time = loggingEvent.TimeStamp.ToString("yyyyMMdd HHmmss.fff zzz"),
						machine = Environment.MachineName,
                        logger = loggingEvent.LoggerName,
						process = loggingEvent.Domain,
						thread = loggingEvent.ThreadName,
						message = string.IsNullOrWhiteSpace(message) ? null : message,
						ex = string.IsNullOrWhiteSpace(exceptionString) ? null : exceptionString,
			       	};
		}
	}
}