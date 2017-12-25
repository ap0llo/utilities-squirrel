using System;

namespace Grynwald.Utilities.Squirrel.Installation
{
    class ExceptionCallbackInstallerStep : IInstallerStep
    {
        readonly IInstallerStep m_InnerStep;
        readonly Action<Exception> m_OnException;


        public ExceptionCallbackInstallerStep(IInstallerStep innerStep, Action<Exception> onException)
        {
            m_InnerStep = innerStep ?? throw new ArgumentNullException(nameof(innerStep));
            m_OnException = onException ?? throw new ArgumentNullException(nameof(onException));
        }


        public void OnInitialInstall(Version version) => Execute(() => m_InnerStep.OnInitialInstall(version));

        public void OnAppUpdate(Version version) => Execute(() => m_InnerStep.OnAppUpdate(version));

        public void OnAppUninstall(Version version) => Execute(() => m_InnerStep.OnAppUninstall(version));


        void Execute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                // call exception callback and rethrow exception
                m_OnException(e);
                throw;
            }
        }
    }
}