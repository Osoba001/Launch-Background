using System.Diagnostics;

namespace Cyphercrescent.SelfService.BuildBackgroundService.FileManager
{

    public class DownloadFileManager
    {
        private FileInfo? file = default;
        private readonly int delayPeriod = 10000;
        FileSystemWatcher watcher = new();

        public void StartWatching()
        {
            watcher = new()
            {
                Path = SourceFolder,
                EnableRaisingEvents = true
            };
            watcher.Created += CreateHandler;
        }

        public async void CreateHandler(object sender, FileSystemEventArgs e)
        {

            string fileFullPath = e.FullPath;
            //If the download process is not in progress, skip the 
            await WaitForDownloadProcessToComplete(fileFullPath);
            //Launch the downloaded file.
            Process.Start(fileFullPath);

        }

        private async Task WaitForDownloadProcessToComplete(string fileFullPath)
        {
            do
            {
                await Task.Delay(delayPeriod);
            } while (IsFileBusy(fileFullPath));
        }

        public void Stop()
        {
            watcher.Dispose();
        }
        public bool IsFileBusy(string filePath)
        {
            try
            {
                file = new FileInfo(filePath);
                using FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                stream.Dispose();
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
