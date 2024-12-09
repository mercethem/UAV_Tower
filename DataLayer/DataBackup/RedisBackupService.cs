using System.Diagnostics;


namespace DataBackup
{
    public class RedisBackupService : IBackupService
    {
        public void TakeBackup(string backupLocation)
        {
            if (!IsContainerRunning("my-redis"))
            {
                Console.WriteLine("Redis container is not running. Backup will not be performed."); // Checking if the container is running // Konteynerin çalışıp çalışmadığını kontrol ediyoruz
                return;
            }

            Console.WriteLine($"Redis backup is being taken and copied to {backupLocation}...");

            // Starting the backup
            Console.WriteLine("Redis backup started...");
            RunCommand("docker", $"exec my-redis redis-cli BGSAVE");

            // Copying the backup file from the container to the specified location
            RunCommand("docker", $"cp my-redis:/data/dump.rdb {backupLocation}\\redis");

            // Backup completed
            Console.WriteLine("Redis backup completed.");
        }

        private void RunCommand(string command, string arguments)
        {
            ProcessStartInfo pro = new ProcessStartInfo(command, arguments)
            {
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process process = Process.Start(pro);
            process.WaitForExit(); // Wait for the command to complete // Komutun tamamlanmasını bekliyoruz
        }

        private bool IsContainerRunning(string containerName)
        {
            var result = RunCommandAndGetOutput("docker", $"ps -q -f name={containerName}");
            return !string.IsNullOrEmpty(result); // Checking if the container is running // Konteynerin çalışıp çalışmadığını kontrol ediyoruz
        }

        private string RunCommandAndGetOutput(string command, string arguments)
        {
            ProcessStartInfo pro = new ProcessStartInfo(command, arguments)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = Process.Start(pro);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
    }
}
