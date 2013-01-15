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
		private Process _currentProcess;

		public LogglyFormatter()
		{
			_currentProcess = Process.GetCurrentProcess();
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
			if (string.IsNullOrWhiteSpace(exceptionString))
			{
				exceptionString = null; //ensure empty strings aren't included in the json output.
			}
			return new
			       	{
			       		level = loggingEvent.Level.DisplayName,
			       		time = loggingEvent.TimeStamp.ToString("yyyyMMdd HHmmss.fff zzz"),
						machine = Environment.MachineName,
                        logger = loggingEvent.LoggerName,
						process = _currentProcess.ProcessName,
						thread = loggingEvent.ThreadName,
						message = loggingEvent.MessageObject,
						ex = exceptionString,
			       	};
		}
	}
}