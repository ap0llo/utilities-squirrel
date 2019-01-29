using System.IO;
using System.Reflection;
using System;

namespace Grynwald.Utilities.Squirrel.Installation
{
    class SaveResourceToFileInstallerStep : IInstallerStep
    {
        readonly Assembly m_ResourceAssembly;
        readonly string m_ResourceName;
        readonly Func<string> m_GetFilePath;
        readonly bool m_OverwriteOnUpdate;


        public SaveResourceToFileInstallerStep(Assembly resourceAssembly, string resourceName, Func<string> getFilePath, bool overwriteOnUpdate)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentException("Value must not be null or empty", nameof(resourceName));

            if (getFilePath == null)
                throw new ArgumentNullException(nameof(getFilePath));

            m_ResourceAssembly = resourceAssembly ?? throw new ArgumentNullException(nameof(resourceAssembly));
            m_ResourceName = resourceName;
            m_GetFilePath = getFilePath;
            m_OverwriteOnUpdate = overwriteOnUpdate;
        }


        public void OnInitialInstall(Version version) => 
            SaveResource(m_ResourceName, m_GetFilePath(), overwrite: true);

        public void OnAppUpdate(Version version) => 
            SaveResource(m_ResourceName, m_GetFilePath(), overwrite: m_OverwriteOnUpdate);

        public void OnAppUninstall(Version version) => 
            File.Delete(m_GetFilePath());


        void SaveResource(string resourceName, string path, bool overwrite)
        {
            if (File.Exists(path) && !overwrite)
                return;

            using (var stream = m_ResourceAssembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                File.WriteAllText(path, content);
            }
        }
    }
}