﻿namespace Programs
{
    class TEST
    {
        static void Main(string[] args)
        {
            string programPath = @"C:\Users\merce\OneDrive\Masaüstü\dump1090-win.1.10.3010.14";
            string programName = @"dump1090.bat"; //==> Be carefull that name is .bat
            string programPath2 = @"C:\Program Files (x86)\VirtualRadar";
            string programName2 = @"VirtualRadar.exe";
            string command = ".exe --interactive --net --net-ro-size 500 --net-ro-rate 5 --net-buffer 5 --net-beast --mlat";
            Programs dump1090 = new Programs(programName, programPath);
            Programs radar = new Programs(programName2, programPath2);

            while (true)
            {
                Console.WriteLine("Lütfen bir sayı girin (1, 2):");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    dump1090.Run(command);
                    Task.Delay(2000).Wait();
                    radar.Run();
                }
                else if (input == "2")
                {
                    dump1090.Stop("dump1090.exe");
                    radar.Stop(programName2);
                }
                else
                {
                    break;
                }
            }
        }
    }

}