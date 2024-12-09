using System.Diagnostics;


namespace DataBackup
{
    public class PostgreSQLBackupService : IBackupService
    {
        public void TakeBackup(string backupLocation)
        {
            if (!IsContainerRunning("my-postgres"))
            {
                Console.WriteLine("PostgreSQL container is not running. Backup will not be performed.");
                return;
            }

            Console.WriteLine($"PostgreSQL backup is being taken and copied to {backupLocation}...");

            // Starting the backup
            Console.WriteLine("PostgreSQL backup started...");

            // Make sure the database name is correct
            string databaseName = "postgres";  // Ensure you're using the correct database name
            RunCommand("docker", $"exec my-postgres pg_dump -U postgres -F c -b -v -f /tmp/backup.dump {databaseName}");

            // Copying backup file from the container to the specified location
            RunCommand("docker", $"cp my-postgres:/tmp/backup.dump {backupLocation}\\postgresql");

            // Backup completed
            Console.WriteLine("PostgreSQL backup completed.");
        }

        private void RunCommand(string command, string arguments)
        {
            ProcessStartInfo pro = new ProcessStartInfo(command, arguments)
            {
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process process = Process.Start(pro);
            process.WaitForExit(); // Waiting for the command to complete // Komutun tamamlanmasını bekliyoruz
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
