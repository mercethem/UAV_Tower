using System.Diagnostics;// Using System.Diagnostics namespace for process management // Süreç yönetimi için System.Diagnostics ad alanını kullanma

namespace Programs
{
    public abstract class BasePrograms : IPrograms // Abstract class implementing IProgram // IProgram'ı uygulayan soyut sınıf
    {
        protected string programName { get; } // Property for program name // Program adı için özellik
        protected string programPath { get; } // Property for program path // Program yolu için özellik

        public BasePrograms(string programName, string programPath) // Constructor for initializing program name and path // Program adı ve yolunu başlatan yapılandırıcı
        {
            this.programName = programName;
            this.programPath = programPath;
        }

        public abstract ProcessStartInfo CreateProcessStartInfo(string programName); // Abstract method to be implemented in derived classes // Türetilmiş sınıflarda uygulanacak soyut yöntem

        public void Run() // Method to run the program // Programı çalıştıran yöntem
        {
            ProcessStartInfo processInfo = CreateProcessStartInfo(this.programName); // Create process start info for the program name // Program adı için işlem başlatma bilgisi oluştur
            try
            {
                Process.Start(processInfo); // Start the process // İşlemi başlat
            }
            catch (Exception ex) // Catch block for handling exceptions // İstisnaları ele alan yakalama bloğu
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}"); // Print error message in Turkish // Hata mesajını Türkçe yazdır
            }
        }

        public void Run(string command) // Method to run the program with a command // Programı bir komut ile çalıştıran yöntem
        {
            ProcessStartInfo processInfo = CreateProcessStartInfo(this.programName + " " + command); // Create process start info with command // Komut ile işlem başlatma bilgisi oluştur
            try
            {
                Process.Start(processInfo); // Start the process // İşlemi başlat
            }
            catch (Exception ex) // Catch block for handling exceptions // İstisnaları ele alan yakalama bloğu
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}"); // Print error message in Turkish // Hata mesajını Türkçe yazdır
            }
        }

        public void Stop(string programName) // Method to stop a process by its name // Bir işlemi adıyla durduran yöntem
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("taskkill", $"/F /IM {programName}") // Setup to kill the process using taskkill command // İşlemi taskkill komutunu kullanarak durdurmak için ayarlama
                {
                    RedirectStandardOutput = true, // Redirect standard output // Standart çıktıyı yönlendirme
                    RedirectStandardError = true, // Redirect standard error // Standart hatayı yönlendirme
                    UseShellExecute = false, // Do not use shell to start the process // İşlemi başlatmak için shell kullanma
                    CreateNoWindow = true // Do not create a window // Bir pencere oluşturma
                };

                using (Process process = Process.Start(startInfo)) // Start the process // İşlemi başlat
                {
                    process.WaitForExit(); // Wait for the process to exit // İşlemin çıkmasını bekle
                    string output = process.StandardOutput.ReadToEnd(); // Read standard output // Standart çıktıyı oku
                    string error = process.StandardError.ReadToEnd(); // Read standard error // Standart hatayı oku

                    if (!string.IsNullOrEmpty(output)) // Check if output is not empty // Çıktının boş olmadığını kontrol et
                    {
                        Console.WriteLine(output); // Print the output // Çıktıyı yazdır
                    }

                    if (!string.IsNullOrEmpty(error)) // Check if error is not empty // Hatanın boş olmadığını kontrol et
                    {
                        Console.WriteLine($"Hata: {error}"); // Print the error message in Turkish // Hata mesajını Türkçe yazdır
                    }
                }
            }
            catch (Exception ex) // Catch block for handling exceptions // İstisnaları ele alan yakalama bloğu
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}"); // Print error message in Turkish // Hata mesajını Türkçe yazdır
            }
        }
    }
}