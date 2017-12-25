using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Grynwald.Utilities.Squirrel.Installation
{
    /// <summary>
    /// Builder class to ease instantiation of new <see cref="Installer"/> instances
    /// </summary>
    public class InstallerBuilder
    {        
        readonly LinkedList<IInstallerStep> m_Steps = new LinkedList<IInstallerStep>();
        Action m_OnFirstRun;
        Action<Exception> m_OnException;


        private InstallerBuilder()
        {
            // InstallationFlagFileInstallerStep has to be the first step
            // because ApplicationInfo depends on it to determine the application
            // root directory
            m_Steps.AddFirst(new InstallationFlagFileInstallerStep());

            // Squirrel catches all exceptions thrown by install events
            // by default, log exception before Squirrel catches them
            // this exception handler can be overwritten using the OnException() builder method
            m_OnException = (Exception e) =>
            {
                try
                {
                    using (var stream = File.OpenWrite(Path.Combine(ApplicationInfo.GetDirectoryPath(SpecialDirectory.CurrentVersionRootDirectory), $"Exception_{Guid.NewGuid()}.txt")))
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(DateTime.Now);
                        writer.Write(e);
                    }
                }
                catch
                {
                    // ignore
                }
            };
        }


        /// <summary>
        /// Adds the specified step to the installer
        /// </summary>
        public InstallerBuilder AddCustomStep(IInstallerStep step)
        {
            m_Steps.AddLast(step ?? throw new ArgumentNullException(nameof(step)));
            return this;
        }

        /// <summary>
        /// Adds a step that adds the specified directory to the user's PATH environment variable to the installer
        /// </summary>
        public InstallerBuilder AddDirectoryToPath(SpecialDirectory directory)
        {
            m_Steps.AddLast(new AddDirectoryToPathInstallerStep(() => ApplicationInfo.GetDirectoryPath(directory)));
            return this;
        }

        /// <summary>
        /// Adds a step that adds the specified directory to the user's PATH environment variable to the installer
        /// </summary>
        /// <returns></returns>
        public InstallerBuilder AddDirectoryToPath(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Value must not be null or empty", nameof(directory));

            m_Steps.AddLast(new AddDirectoryToPathInstallerStep(() => directory));
            return this;
        }
        
        /// <summary>
        /// Adds a step to the installer that creates a new batch-file at the specified location with the specified command
        /// </summary>
        /// <param name="directory">The directory to create the batch-file in</param>
        /// <param name="fileName">The file name of the batch file</param>
        /// <param name="command">The command that is to be executed by the batch file</param>
        /// <returns></returns>
        public InstallerBuilder CreateBatchFile(SpecialDirectory directory, string fileName, string command)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Value must not be null or empty", nameof(fileName));

            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentException("Value must not be null or empty", nameof(command));

            m_Steps.AddLast(
                new CreateBatchFileInstallerStep(
                    () => Path.Combine(ApplicationInfo.GetDirectoryPath(directory), fileName), 
                    command));
            return this;
        }

        /// <summary>
        /// Adds a step to the installer that creates a new batch-file at the specified location with the specified command
        /// </summary>
        /// <param name="batchFilePath">The full path of the batch file to create</param>
        /// <param name="command">The command that is to be executed by the batch file</param>
        /// <returns></returns>
        public InstallerBuilder CreateBatchFile(string batchFilePath, string command)
        {
            if (string.IsNullOrWhiteSpace(batchFilePath))
                throw new ArgumentException("Value must not be null or empty", nameof(batchFilePath));

            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentException("Value must not be null or empty", nameof(command));

            m_Steps.AddLast(new CreateBatchFileInstallerStep(() => batchFilePath, command));
            return this;
        }

        /// <summary>
        /// Saves the specified embedded resource to a file
        /// </summary>
        /// <param name="resourceAssembly">The assembly that contains the resource</param>
        /// <param name="resourceName">The name of the resource </param>
        /// <param name="filePath">The full path of the file the resource will be written to</param>
        /// <param name="overwriteOnUpdate">Specifies whether the file should be overwritten with the resource contents during application updates</param>        
        public InstallerBuilder SaveResourceToFile(Assembly resourceAssembly, string resourceName, string filePath, bool overwriteOnUpdate)
        {
            if (resourceAssembly == null)
                throw new ArgumentNullException(nameof(resourceAssembly));

            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentException("Value must not be null or empty", nameof(resourceName));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Value must not be null or empty", nameof(filePath));

            m_Steps.AddLast(new SaveResourceToFileInstallerStep(resourceAssembly, resourceName, () => filePath, overwriteOnUpdate));
            return this;
        }

        /// <summary>
        /// Saves the specified embedded resource to a file
        /// </summary>
        /// <param name="resourceAssembly">The assembly that contains the resource</param>
        /// <param name="resourceName">The name of the resource </param>
        /// <param name="directory">The directory to place the file in</param>
        /// <param name="fileName">The name of the file the resource will be written to</param>
        /// <param name="overwriteOnUpdate">Specifies whether the file should be overwritten with the resource contents during application updates</param>        
        /// <returns></returns>
        public InstallerBuilder SaveResourceToFile(Assembly resourceAssembly, string resourceName, SpecialDirectory directory, string fileName, bool overwriteOnUpdate)
        {
            if (resourceAssembly == null)
                throw new ArgumentNullException(nameof(resourceAssembly));

            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentException("Value must not be null or empty", nameof(resourceName));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Value must not be null or empty", nameof(fileName));

            
            m_Steps.AddLast(
                new SaveResourceToFileInstallerStep(
                    resourceAssembly, 
                    resourceName, 
                    () => Path.Combine(ApplicationInfo.GetDirectoryPath(directory), fileName), 
                    overwriteOnUpdate));
            return this;
        }

        /// <summary>
        /// Sets the action to be run after the application has been launched the first time
        /// </summary>
        public InstallerBuilder OnFirstRun(Action action)
        {
            m_OnFirstRun = action;
            return this;
        }

        /// <summary>
        /// Sets the action to be executed when an execption occurrs during installation
        /// </summary>
        public InstallerBuilder OnException(Action<Exception> onException)
        {
            m_OnException = onException ?? throw new ArgumentNullException(nameof(onException));
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="Installer"/> instance with the configured steps
        /// </summary>
        /// <returns></returns>
        public Installer Build() => 
            new Installer(
                m_OnFirstRun,
                new ExceptionCallbackInstallerStep(
                    new CompositeInstallerStep(m_Steps.ToArray()),
                    m_OnException));
       

        /// <summary>
        /// Creates a new, empty <see cref="InstallerBuilder"/>
        /// </summary>
        public static InstallerBuilder CreateBuilder() => new InstallerBuilder();

        /// <summary>
        /// Creates a new <see cref="InstallerBuilder"/> with preconfigured default steps for installation of console applications.
        /// 
        /// By default, the installer will 
        ///     - create a batch file in the application root directory (see <see cref="SpecialDirectory.ApplicationRootDirectory"/>)
        ///       that launches the applications
        ///     - add the application root directory to the user's PATH environment variable
        ///     - On first run, display a console window with a message indicating that the application was installed successfully
        /// </summary>
        /// <returns></returns>
        public static InstallerBuilder CreateConsoleApplicationBuilder() =>
             CreateBuilder()
                .CreateBatchFile(SpecialDirectory.ApplicationRootDirectory, ApplicationInfo.ApplicationName + ".bat", Assembly.GetEntryAssembly().Location)
                .AddDirectoryToPath(SpecialDirectory.ApplicationRootDirectory)
                .OnFirstRun(() =>
                {
                    Console.WriteLine($"{ApplicationInfo.ApplicationName} was installed");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Environment.Exit(0);
                });
        
    }
}