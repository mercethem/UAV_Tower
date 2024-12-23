using System;
using System.Diagnostics;

namespace Programs
{
    public abstract class BasePrograms : IPrograms
    {
        protected string programName { get; } // Property for program name 
        protected string programPath { get; } // Property for program path 

        public BasePrograms(string programName, string programPath) // Constructor for initializing program name and path
        {
            this.programName = programName;
            this.programPath = programPath;
        }

        public abstract ProcessStartInfo CreateProcessStartInfo(string programName); // Abstract method to be implemented in derived classes 

        public void Run() // Method to run the program // Programı çalıştıran yöntem
        {
            ProcessStartInfo processInfo = CreateProcessStartInfo(this.programName); // Create process start info for the program name 
            try
            {
                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
            }
        }

        public void Run(string command) // Method to run the program with a command 
        {
            ProcessStartInfo processInfo = CreateProcessStartInfo(this.programName + " " + command); // Create process start info with command 
            try
            {
                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
            }
        }

        public void Stop(string programName) // Method to stop a process by its name
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("taskkill", $"/F /IM {programName}") // Setup to kill the process using taskkill command
                {
                    RedirectStandardOutput = true, // Redirect standard output 
                    RedirectStandardError = true, // Redirect standard error 
                    UseShellExecute = false, // Do not use shell to start the process
                    CreateNoWindow = true // Do not create a window 
                };

                using (Process process = Process.Start(startInfo)) // Start the processat
                {
                    process.WaitForExit(); // Wait for the process to exit 
                    string output = process.StandardOutput.ReadToEnd(); // Read standard output 
                    string error = process.StandardError.ReadToEnd(); // Read standard error 

                    if (!string.IsNullOrEmpty(output))
                    {
                        Console.WriteLine(output);
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Hata: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bir hata oluştu: {ex.Message}");
            }
        }
    }
}