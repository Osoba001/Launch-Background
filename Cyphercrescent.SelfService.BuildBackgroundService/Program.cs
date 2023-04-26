using Cyphercrescent.SelfService.BuildBackgroundService.FileManager;
using Topshelf;
/// <summary>
/// This is the entering point of the application.
/// Every methods in this class runs at start up.
/// </summary>
class Program
{
    /// <summary>
    /// This method uses Topshelf to start and stop the windows background service
    /// by calling the StartWatching and Stop methods in the DownloadFolderSelfService class.
    /// </summary>
    static void Main()
    {
        var exitCode = HostFactory.Run(x =>
        {
            x.Service<DownloadFileManager>(s =>
            {
                s.ConstructUsing(sv => new DownloadFileManager());
                s.WhenStarted(sv => sv.StartWatching());
                s.WhenStopped(sv => sv.Stop());
            });
            x.RunAsLocalSystem();

            x.SetServiceName("CypherCrescent-Build-Service");
            x.SetDisplayName("CypherCrescent Build Service");
            x.SetDescription("A Service that create a new build and launch the .exe file for every new download");
        });

        int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
        Environment.ExitCode = exitCodeValue;
    }
}