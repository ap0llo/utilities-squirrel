using System;
using Squirrel;

namespace Grynwald.Utilities.Squirrel.Installation
{
    /// <summary>
    /// Encapsulates the set of steps to be taken when installing, updatring or uninstalling the application
    /// </summary>
    public class Installer
    {
        readonly Action m_OnFirstRun;
        readonly IInstallerStep[] m_Steps;


        internal Installer(Action onFirstRun, params IInstallerStep[] steps)
        {
            m_Steps = steps ?? throw new ArgumentNullException(nameof(steps));
            m_OnFirstRun = onFirstRun;
        }

        
        /// <summary>
        /// Handles installation events (this method should be called in the application's
        /// Main method as early as possible)
        /// </summary>
        /// <remarks>
        /// See also <see cref="SquirrelAwareApp.HandleEvents(Action{Version}, Action{Version}, Action{Version}, Action{Version}, Action, string[])"/>
        /// </remarks>
        public void HandleInstallationEvents()
        {
            var compositeStep = new CompositeInstallerStep(m_Steps);

            SquirrelAwareApp.HandleEvents(
                onInitialInstall: compositeStep.OnInitialInstall,
                onAppUpdate: compositeStep.OnAppUpdate,
                onAppUninstall: compositeStep.OnAppUninstall,
                onFirstRun: m_OnFirstRun);
        }
    }
}