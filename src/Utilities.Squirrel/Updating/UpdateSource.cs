namespace Grynwald.Utilities.Squirrel.Updating
{
    /// <summary>
    /// Specifies constants that represent the different types of update sources
    /// that can be used by the updater
    /// </summary>
    public enum UpdateSource
    {
        /// <summary>
        /// Unconfigured update source
        /// </summary>
        NotConfigured = 0,

        /// <summary>
        /// Update from GitHub releases
        /// </summary>
        GitHub = 1,

        /// <summary>
        /// Uppdate from local file system (or a network share)
        /// </summary>
        FileSystem = 2
    }
}