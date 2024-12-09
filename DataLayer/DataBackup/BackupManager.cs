namespace DataBackup
{
    public class BackupManager
    {
        private readonly IBackupService[] _backupServices;

        public BackupManager(IBackupService[] backupServices)
        {
            _backupServices = backupServices; // Assigning backup services to the manager // Yedekleme servislerini yöneticisine atıyoruz
        }

        public void Start()
        {
            Console.WriteLine("Backup operation will start. Press any key to exit..."); // Starting the backup process, waiting for key press // Yedekleme işlemi başlatılıyor, tuşa basılmasını bekliyoruz
            PerformBackup();
        }

        private void PerformBackup()
        {
            var currentDate = DateTime.Now;
            Console.WriteLine($"Backup started: {currentDate}"); // The backup operation is starting // Yedekleme işlemi başladı

            // Creating a new folder with the current date and time using the format yyyy/MM/dd_HH-mm-ss
            string folderName = currentDate.ToString("yyyy/MM/dd_HH:mm:ss").Replace(":", "-");
            string fullBackupLocation = Path.Combine(BackupLocation, folderName);

            // Create the directory if it does not exist
            if (!Directory.Exists(fullBackupLocation))
            {
                Directory.CreateDirectory(fullBackupLocation); // Creating the directory // Dizini oluşturuyoruz
            }

            // Taking backup from each backup service and saving it to the newly created folder
            foreach (var backupService in _backupServices)
            {
                backupService.TakeBackup(fullBackupLocation); // Send the new backup location with the folder // Yeni yedekleme dizinini gönderiyoruz
            }

            Console.WriteLine("Backup operation completed."); // Backup process completed // Yedekleme işlemi tamamlandı
        }

        // Specifying the backup directory here
        public static string BackupLocation { get; set; } // Yedekleme dizini burada belirtiliyor // Specifying the backup directory here
    }
}
