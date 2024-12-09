using System.Diagnostics;


namespace DataBackup
{
    public class MongoDBBackupService : IBackupService
    {
        public void TakeBackup(string backupLocation)
        {
            if (!IsContainerRunning("my-mongodb"))
            {
                Console.WriteLine("MongoDB container is not running. Backup will not be performed.");
                return;
            }

            Console.WriteLine($"MongoDB backup is being taken and copied to {backupLocation}...");

            // Starting the backup
            Console.WriteLine("MongoDB backup started...");
            RunCommand("docker", $"exec my-mongodb mongodump --archive={backupLocation}\\mongodb\\backup.archive");

            // Alternatively, you can copy the database dump to the host machine
            RunCommand("docker", $"cp my-mongodb:/data/db {backupLocation}\\mongodb");

            // Backup completed
            Console.WriteLine("MongoDB backup completed.");
        }

        private void RunCommand(string command, string arguments)
        {
            ProcessStartInfo pro = new ProcessStartInfo(command, arguments)
            {
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process process = Process.Start(pro);
            process.WaitForExit(); // Waiting for the command to finish // Komutun tamamlanmasını bekliyoruz
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
