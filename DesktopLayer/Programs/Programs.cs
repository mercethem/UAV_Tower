using System.Diagnostics;

namespace Programs
{
    public class Programs : BasePrograms, IPrograms
    {
        private string programName { get; }
        private string programPath { get; }
        private string command { get; }

        public Programs(string programName, string programPath) : base(programName, programPath) // Constructor initializing base class with program name and path
        {
            this.programName = programName; // Set program name 
            this.programPath = programPath; // Set program path 
        }

        public override ProcessStartInfo CreateProcessStartInfo(string programName) // Implement the method to create process start info 
        {
            return new ProcessStartInfo("cmd.exe", "/c" + " " + programName) // Create a new ProcessStartInfo for cmd.exe 
            {
                UseShellExecute = true, // Use shell to start the process 
                WorkingDirectory = this.programPath, // Set the working directory 
                CreateNoWindow = false, // Do not create a window 
                WindowStyle = ProcessWindowStyle.Hidden // Set the window style to hidden 
            };
        }
    }
}