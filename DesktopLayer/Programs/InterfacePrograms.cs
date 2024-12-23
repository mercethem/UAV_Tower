using System.Diagnostics;

namespace Programs
{
    public interface IPrograms
    {
        void Run(); // Method to run the program without parameters 
        void Run(string command); // Method to run the program with a command
        void Stop(string processName); // Method to stop a running process by its name
        ProcessStartInfo CreateProcessStartInfo(string command); // Method to create process start info
    }
}
