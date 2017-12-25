using Grynwald.Utilities.Squirrel.Installation;
using System;
using System.IO;
using System.Reflection;

namespace Grynwald.Utilities.Squirrel
{
    /// <summary>
    /// Provides information about the application
    /// </summary>
    public static class ApplicationInfo
    {
        /// <summary>
        /// The name of the application (the name of the entry assembly)
        /// </summary>
        public static string ApplicationName => Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        /// Determines whether the application is running from a squirrel-based installation.
        /// This is determined by a "flag" file placed next to the application by the installer during installation 
        /// </summary>
        /// <remarks>
        /// Be careful to use this property during installation or before the installer has been called,
        /// as this will return false if the installation step creating the "flag" file has not yet run
        /// </remarks>
        public static bool IsInstalled => InstallationFlagFileInstallerStep.IsInstalled;

        
        /// <summary>
        /// Gets the path of the specified <see cref="SpecialDirectory"/>
        /// </summary>
        public static string GetDirectoryPath(SpecialDirectory directory)
        {           
            switch(directory)
            {
                case SpecialDirectory.ApplicationRootDirectory:
                    return IsInstalled
                        ? Path.GetFullPath(Path.Combine(GetEntryAssemblyDirectory(), "..")).TrimEnd(Path.DirectorySeparatorChar)
                        : GetEntryAssemblyDirectory();
                    
                case SpecialDirectory.CurrentVersionRootDirectory:
                    return GetEntryAssemblyDirectory();

                default:
                    throw new NotImplementedException($"Unimplemented case in switch-Statement: {nameof(SpecialDirectory)}.{directory}");
            }
        }


        static string GetEntryAssemblyDirectory()
        {
            var assemblyLocation = Assembly.GetEntryAssembly().Location;
            return Path.GetDirectoryName(assemblyLocation).TrimEnd(Path.DirectorySeparatorChar);
        }
    }
}