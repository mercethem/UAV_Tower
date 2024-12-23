namespace DataBackup
{
    public interface IBackupService
    {
        void TakeBackup(string backupLocation); // We take the backup location as a parameter
    }
}
