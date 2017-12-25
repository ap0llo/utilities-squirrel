using System;

namespace Grynwald.Utilities.Squirrel.Installation
{
    /// <summary>
    /// Represents a step executing duing installation/updating or uninstallation of the application
    /// </summary>
    public interface IInstallerStep
    {
        /// <summary>
        /// The action to perfom when installing the application
        /// </summary>        
        void OnInitialInstall(Version version);

        /// <summary>
        /// The action to perform when updating the application
        /// </summary>
        /// <param name="version"></param>
        void OnAppUpdate(Version version);

        /// <summary>
        /// The action to perform when uninstalling the application
        /// </summary>
        void OnAppUninstall(Version version);
    }
}