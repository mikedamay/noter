using System;
using System.IO;

namespace noter.Common
{
    /// <inheritdoc/>/>
    public class UnsupportedPlatformException : Exception
    {
        
    }

    internal class OsDetector
    {
        // TODO use System.Runtime.InteropServices.RuntimeInformation.Platform when the position
        // is clear
        public Os DetectOs()
        {
            // https://stackoverflow.com/questions/38790802/determine-operating-system-in-net-core
            // thanks to: https://stackoverflow.com/users/3325704/jariq with amendments by me
            Os os;
            string windir = Environment.GetEnvironmentVariable("windir");
            if (!string.IsNullOrEmpty(windir) && windir.Contains(@"\") && Directory.Exists(windir))
            {
                os = Os.Windows;
            }
            else if (File.Exists(@"/proc/sys/kernel/ostype"))
            {
                string osType = File.ReadAllText(@"/proc/sys/kernel/ostype");
                if (osType.StartsWith("Linux", StringComparison.OrdinalIgnoreCase))
                {
                    // Note: Android gets here too
                    os = Os.Linux;
                }
                else
                {
                    throw new UnsupportedPlatformException();
                }
            }
            else if (File.Exists(@"/System/Library/CoreServices/SystemVersion.plist"))
            {
                // Note: iOS gets here too
                os = Os.MacOS;
            }
            else
            {
                throw new UnsupportedPlatformException();
            }
            return os;
        }
    }
}

namespace noter.Common
{
    public enum Os
    {
        /// <summary>
        /// Beans typically have an OS of OS.Any
        /// this will match any OS under which the container is executing
        /// </summary>
        Any,
        /// <summary>
        /// Any version of Linux supporting dotnetstandard 2.0
        /// </summary>
        Linux,
        /// <summary>
        /// Any version of Windows supporting dotnetstandard 2.0
        /// </summary>
        Windows,
        /// <summary>
        /// Any MAC version supported by dotnetstandard 2.0
        /// </summary>
        MacOS
    }
}