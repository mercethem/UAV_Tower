namespace DataBackup
{
    class BackupProcessor
    {
        //start the backup process
        public void StartBackup(string backupLocation)
        {
            // If the user doesn't provide a location, use a default directory
            if (string.IsNullOrEmpty(backupLocation))
            {
                backupLocation = @"C:\Backup";
                Console.WriteLine($"Using the default backup directory: {backupLocation}");
            }

            BackupManager.BackupLocation = backupLocation;

            // Starting backup services
            IBackupService redisBackup = new RedisBackupService();
            IBackupService mongoBackup = new MongoDBBackupService();
            IBackupService postgresqlBackup = new PostgreSQLBackupService();

            // Starting the backup manager with the services
            var backupServices = new IBackupService[] { redisBackup, mongoBackup, postgresqlBackup };
            var backupManager = new BackupManager(backupServices);
            backupManager.Start();
        }
    }
}
