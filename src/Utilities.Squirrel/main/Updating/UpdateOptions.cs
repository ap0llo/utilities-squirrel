using System;

namespace Grynwald.Utilities.Squirrel.Updating
{
    /// <summary>
    /// Option class to configure <see cref="Updater"/>
    /// </summary>
    public sealed class UpdateOptions
    {
        string m_Path;

        /// <summary>
        /// Enables or disables updating
        /// </summary>
        public bool Enable { get; set; } = false;

        /// <summary>
        /// Specifies the type of update source to use
        /// </summary>
        public UpdateSource Source { get; set; } = UpdateSource.NotConfigured;

        /// <summary>
        /// The path to load updates from.
        /// When the configured source is FileSystem, this is the path of the directory containing the releases,
        /// when updateing from GitHub releases, this is the url of the github repository to get updates from
        /// </summary>
        public string Path
        {
            get => m_Path;
            set => m_Path = Environment.ExpandEnvironmentVariables(value);
        }

        /// <summary>
        /// Specifies whether pre-release versions should be installed
        /// </summary>
        public bool InstallPreReleaseVersions { get; set; } = false;

        /// <summary>
        /// Specifies the interval between two checks for updates
        /// </summary>
        public TimeSpan Interval { get; set; } = TimeSpan.FromHours(1);


        /// <summary>
        /// Initializes a new instance of <see cref="UpdateOptions"/>
        /// </summary>
        public UpdateOptions()
        {
            Path = "";
        }

    }    
}