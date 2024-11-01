using System.Diagnostics;// Using System.Diagnostics namespace for process management // Süreç yönetimi için System.Diagnostics ad alanını kullanma

namespace Programs
{
    public class Programs : BasePrograms, IPrograms // Class extending BaseProgram and implementing IProgram // BaseProgram'dan türeyen ve IProgram'ı uygulayan sınıf
    {
        private string programName { get; } // Property for program name (private) // Program adı için özel özellik
        private string programPath { get; } // Property for program path (private) // Program yolu için özel özellik
        private string command { get; } // Property for command (private) // Komut için özel özellik

        public Programs(string programName, string programPath) : base(programName, programPath) // Constructor initializing base class with program name and path // Taban sınıfı yapılandırıcısını program adı ve yolu ile başlatan yapılandırıcı
        {
            this.programName = programName; // Set program name // Program adını ayarla
            this.programPath = programPath; // Set program path // Program yolunu ayarla
        }

        public override ProcessStartInfo CreateProcessStartInfo(string programName) // Implement the method to create process start info // İşlem başlatma bilgisi oluşturma yöntemini uygulama
        {
            return new ProcessStartInfo("cmd.exe", "/c" + " " + programName) // Create a new ProcessStartInfo for cmd.exe // cmd.exe için yeni bir ProcessStartInfo oluştur
            {
                UseShellExecute = true, // Use shell to start the process // İşlemi başlatmak için shell kullan
                WorkingDirectory = this.programPath, // Set the working directory // Çalışma dizinini ayarla
                CreateNoWindow = false, // Do not create a window // Bir pencere oluşturma
                WindowStyle = ProcessWindowStyle.Hidden // Set the window style to hidden // Pencere stilini gizli yap
            };
        }
    }
}