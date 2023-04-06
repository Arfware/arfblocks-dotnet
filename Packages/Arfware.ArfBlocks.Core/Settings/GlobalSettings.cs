using System;

namespace Arfware.ArfBlocks.Core.Settings
{
	internal static class GlobalSettings
	{
		public static LogLevels LogLevel { get; set; } = LogLevels.Debug;

		private static bool IsLoggable(LogLevels logLevel)
		{
			return (int)LogLevel <= (int)logLevel;
		}
		public static bool CanLogDebug()
		{
			return IsLoggable(LogLevels.Debug);
		}
		public static bool CanLogInformation()
		{
			return IsLoggable(LogLevels.Information);
		}
		public static bool CanLogWarning()
		{
			return IsLoggable(LogLevels.Warning);
		}
		public static bool CanLogError()
		{
			return IsLoggable(LogLevels.Error);
		}
	}
}