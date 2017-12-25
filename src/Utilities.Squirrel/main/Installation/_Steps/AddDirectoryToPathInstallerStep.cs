using System;
using System.Collections.Generic;
using System.Linq;

namespace Grynwald.Utilities.Squirrel.Installation
{
    /// <summary>
    /// Installer step that adds the application's installation directory to the user's PATH
    /// </summary>
    class AddDirectoryToPathInstallerStep : IInstallerStep
    {
        readonly Func<string > m_GetDirectory;


        public AddDirectoryToPathInstallerStep(Func<string> getDirectory)
        {
            m_GetDirectory = getDirectory ?? throw new ArgumentNullException(nameof(getDirectory));
        }


        public void OnInitialInstall(Version version)
        {
            var directory = m_GetDirectory();
            var value = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? "";
            var currentValues = new HashSet<string>(value.Split(';'), StringComparer.InvariantCultureIgnoreCase);

            if (!currentValues.Contains(directory))
            {
                Environment.SetEnvironmentVariable("PATH", value + ";" + directory, EnvironmentVariableTarget.User);
            }
        }

        public void OnAppUpdate(Version version)
        {            
        }

        public void OnAppUninstall(Version version)
        {
            var directory = m_GetDirectory();
            var value = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User) ?? "";
            var currentValues = new HashSet<string>(value.Split(';'), StringComparer.InvariantCultureIgnoreCase);

            var valuesToRemove = currentValues.Where(v => StringComparer.InvariantCultureIgnoreCase.Equals(v, directory));
            foreach (var path in valuesToRemove)
            {
                value = value.Replace(path, "");
            }

            while (value.Contains(";;"))
            {
                value = value.Replace(";;", ";");
            }

            Environment.SetEnvironmentVariable("PATH", value, EnvironmentVariableTarget.User);
        }        
    }
}