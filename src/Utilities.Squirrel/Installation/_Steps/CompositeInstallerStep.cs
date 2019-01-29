using System;
using System.Linq;

namespace Grynwald.Utilities.Squirrel.Installation
{
    /// <summary>
    /// Implements a <see cref="IInstallerStep"/> composed of multiple installer steps.    
    /// </summary>
    public class CompositeInstallerStep : IInstallerStep
    {
        readonly IInstallerStep[] m_Steps;


        /// <summary>
        /// Initializes a new instance of <see cref="CompositeInstallerStep"/>
        /// </summary>
        /// <param name="steps"></param>
        public CompositeInstallerStep(params IInstallerStep[] steps)
        {
            m_Steps = steps ?? throw new ArgumentNullException(nameof(steps));
        }


        /// <summary>
        /// Calls the <see cref="IInstallerStep.OnInitialInstall(Version)"/> method of each inner step
        /// </summary>
        public void OnInitialInstall(Version version)
        {
            foreach (var step in m_Steps)
            {
                step.OnInitialInstall(version);
            }
        }

        /// <summary>
        /// Calls the <see cref="IInstallerStep.OnAppUpdate(Version)"/> method of each inner step
        /// </summary>
        public void OnAppUpdate(Version version)
        {
            foreach (var step in m_Steps)
            {
                step.OnAppUpdate(version);
            }
        }

        /// <summary>
        /// Calls the <see cref="IInstallerStep.OnAppUninstall(Version)"/> method of each inner step (in reversed order)
        /// </summary>        
        public void OnAppUninstall(Version version)
        {
            foreach (var step in m_Steps.Reverse())
            {
                step.OnAppUninstall(version);
            }
        }
    }
}