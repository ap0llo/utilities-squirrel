namespace Grynwald.Utilities.Squirrel
{
    /// <summary>
    /// Specifies constants to get well-known application directories
    /// </summary>
    public enum SpecialDirectory
    {
        /// <summary>
        /// The application's root directory. 
        /// If the application is running from an installation, this is %LOCALAPPDATA%\$APPLICATIONNAME,
        /// otherwise it is the directory that contains the application's entry assembly
        /// </summary>
        ApplicationRootDirectory,

        /// <summary>
        /// The root directory for the current version (the the directory that contains the application's entry assembly).
        /// If the application is running from an installation, this is
        /// %LOCALAPPDATA%\$APPLICATIONNAME\app-$VERSION
        /// </summary>
        CurrentVersionRootDirectory
    }
}
