namespace Grynwald.Utilities.Squirrel.Updating
{
    /// <summary>
    /// Defines constants describing the current status of the updater
    /// </summary>
    public enum UpdaterStatus
    {
        /// <summary>
        /// The updater has not been started yet
        /// </summary>
        Initialized = 0,

        /// <summary>
        /// The updater is running
        /// </summary>
        Running,

        /// <summary>
        /// The updater completed successfully
        /// </summary>
        Completed,

        /// <summary>
        /// The updater completed with errors
        /// </summary>
        Failed
    }
}