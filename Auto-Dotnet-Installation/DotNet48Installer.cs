    internal class DotNet48Installer
    {
        public bool InstallationDotNetPhase(List<string> paths)
        {
            try
            {
                string sxsFolderPath = Path.Combine(AppContext.BaseDirectory, paths[0]);
                Process DotNetInstallationProcess = new Process();
                ProcessStartInfo DotNetInstallationProcessInfo = new ProcessStartInfo();
                DotNetInstallationProcessInfo.FileName = "cmd.exe";
                DotNetInstallationProcessInfo.RedirectStandardInput = false;
                DotNetInstallationProcessInfo.Verb = "runas";
                DotNetInstallationProcessInfo.UseShellExecute = true;
                DotNetInstallationProcessInfo.Arguments = "/user:Administrator \"cmd /K " + $" Dism /online /enable-feature /featurename:NetFx4-AdvSrvs /All /Source:\"{sxsFolderPath} \" ";
                DotNetInstallationProcessInfo.CreateNoWindow = true;
                DotNetInstallationProcessInfo.WindowStyle = ProcessWindowStyle.Hidden;
                DotNetInstallationProcess.StartInfo = DotNetInstallationProcessInfo;
                DotNetInstallationProcess.Start();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        internal bool EnableFramework48(List<string> paths)
        {
            return InstallationDotNetPhase(paths);
        }
    }
//for 3.5
    DotNetInstallationProcessInfo.Arguments = "/user:Administrator \"cmd /K " + $" Dism /online /enable-feature /featurename:NetFx3 /All /Source:\"{sxsFolderPath} \" ";