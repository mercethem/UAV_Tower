namespace DataBackup
{
    public class BackupManager
    {
        private readonly IBackupService[] _backupServices;

        public BackupManager(IBackupService[] backupServices)// Assigning backup services to the manager
        {
            _backupServices = backupServices; 
        }

        public void Start()// Starting the backup process, waiting for key press
        {
            Console.WriteLine("Backup operation will start. Press any key to exit..."); 
            PerformBackup();
        }

        private void PerformBackup()// The backup operation is starting
        {
            var currentDate = DateTime.Now;
            Console.WriteLine($"Backup started: {currentDate}"); 

         
            string folderName = currentDate.ToString("yyyy/MM/dd_HH:mm:ss").Replace(":", "-");
            string fullBackupLocation = Path.Combine(BackupLocation, folderName);

            // Create the directory if it does not exist
            if (!Directory.Exists(fullBackupLocation))
            {
                Directory.CreateDirectory(fullBackupLocation); 
            }

            // Taking backup from each backup service and saving it to the newly created folder
            foreach (var backupService in _backupServices)
            {
                backupService.TakeBackup(fullBackupLocation); 
            }

            Console.WriteLine("Backup operation completed."); 
        }

        // Specifying the backup directory here
        public static string BackupLocation { get; set; }
    }
}
