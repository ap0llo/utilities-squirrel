using System;
using System.IO;

namespace Grynwald.Utilities.Squirrel.Installation
{
    class CreateBatchFileInstallerStep : IInstallerStep
    {
        private readonly Func<string> m_GetFileName;
        private readonly string m_Command;
        

        public CreateBatchFileInstallerStep(Func<string> getFileName, string command)
        {
            m_GetFileName = getFileName ?? throw new ArgumentNullException(nameof(getFileName));
            m_Command = command ?? throw new ArgumentNullException(nameof(command));
        }


        public void OnInitialInstall(Version version) => CreateBatchFile();

        public void OnAppUpdate(Version version) => CreateBatchFile();

        public void OnAppUninstall(Version version) => RemoveBatchFile();

        
        void CreateBatchFile()
        {
            using (var fileStream = File.OpenWrite(m_GetFileName()))
            using (var writer = new StreamWriter(fileStream))
            {
                writer.WriteLine("@ECHO OFF");
                writer.WriteLine($"\"{m_Command}\" %*");
            }
        }

        void RemoveBatchFile() => File.Delete(m_GetFileName());
    }
}