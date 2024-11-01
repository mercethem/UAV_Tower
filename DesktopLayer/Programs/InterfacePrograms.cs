using System.Diagnostics; // Using System.Diagnostics namespace for process management // Süreç yönetimi için System.Diagnostics ad alanını kullanma

namespace Programs
{
    public interface IPrograms // Interface defining the program functionalities // Program işlevselliklerini tanımlayan arayüz
    {
        void Run(); // Method to run the program without parameters // Parametre olmadan programı çalıştıran yöntem
        void Run(string command); // Method to run the program with a command // Bir komut ile programı çalıştıran yöntem
        void Stop(string processName); // Method to stop a running process by its name // Bir işlemi adıyla durduran yöntem
        ProcessStartInfo CreateProcessStartInfo(string command); // Method to create process start info // İşlem başlatma bilgisi oluşturan yöntem
    }
}
