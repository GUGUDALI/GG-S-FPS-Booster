using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GG_S_FPS_Booster
{
    class ProcessList
    {
        public int processID { get; set; }
        public ProcessPriorityClass processPriorityClass { get; set; }
        public bool priorityBoostEnabled { get; set; }
        public nint processorAffinity { get; set; }
    }
    internal class Program
    {
        private static int idleCore;
        private static int highCore;
        private static Process[] processes;
        private static int[] cores = new int[Environment.ProcessorCount];
        private static Process fpsProcess;
        private static Process proc;
		private static ProcessList[] processList;
        private static bool isCSFinded = false;

        static void Main(string[] args)
        {
			Console.Title = "GG'S FPS Booster v1.0 by GUGUDALI";
            for (int i = 0; i < cores.Length; i++)
            {
                if (i == 0)
                {
                    cores[i] = 1;
                }
                else
                {
                    cores[i] = cores[i - 1] + cores[i - 1];
                }
            }

            for (int i = 0; i < cores.Length; i++)
            {
                if (i == 0 || i == 1 || i == (cores.Length / 2) || i == cores.Length / 2 + 1)
                {
                    
                }
                else
                {
                    idleCore += cores[i];
                }
            }

            for (int i = 0; i < cores.Length; i++)
            {
                highCore += cores[i];
            }

            while(true){
                try
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("GG'S FPS Booster v1.0 by GUGUDALI\n\n");

                    Console.WriteLine("Waiting for CS2 to start...");
                    while (!isCSFinded){
                        processes = Process.GetProcesses();
                        for (int i = 0; i < processes.Length; i++)
                        {
                            try
                            {
                                proc = processes[i];
                                if (proc.ProcessName == "cs2"){
                                    fpsProcess = proc;
                                    isCSFinded = true;
                                    Console.Clear();
                                    Console.WriteLine("GG'S FPS Booster v1.0 by GUGUDALI\n\n");
                                    Console.WriteLine("CS2 found. ID : " + fpsProcess.Id);
                                }
                            }catch{}
                        }
                        Thread.Sleep(2000);
                    }

                    Console.Clear();
                    Console.WriteLine("GG'S FPS Booster v1.0 by GUGUDALI\n\n");
                    Console.WriteLine("Boosting CS2's FPS...");

                    processes = Process.GetProcesses();
                    processList = new ProcessList[processes.Length];
                    
                    for (int i = 0; i < processes.Length; i++)
                    {
                        try
                        {
                            proc = processes[i];
                                try
                                {
                                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                                    {
                                        processList[i] = new ProcessList{
                                            processID = proc.Id,
                                            processPriorityClass = proc.PriorityClass,
                                            priorityBoostEnabled = proc.PriorityBoostEnabled,
                                            processorAffinity = proc.ProcessorAffinity
                                        };
                                    }else{
                                        processList[i] = new ProcessList{
                                            processID = proc.Id,
                                            processPriorityClass = proc.PriorityClass,
                                            priorityBoostEnabled = proc.PriorityBoostEnabled
                                        };
                                    }
                                }catch{}

                                proc.PriorityClass = ProcessPriorityClass.Idle;
                                proc.PriorityBoostEnabled = false;
                                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && proc.ProcessName != "svchost" && proc.ProcessName != "System" && proc.ProcessName != "Idle" && proc.ProcessName != "csrss" && proc.ProcessName != "smss" && proc.ProcessName != "Registry" && proc.ProcessName != "wininit" && proc.ProcessName != "winlogon" && proc.ProcessName != "lsass" && proc.ProcessName != "lsm" && proc.ProcessName != "cs2")
                                {
                                    proc.ProcessorAffinity = (IntPtr)idleCore;
                                }
                        }catch{}
                    }

                    fpsProcess.PriorityClass = ProcessPriorityClass.RealTime;
                    fpsProcess.PriorityBoostEnabled = true;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        fpsProcess.ProcessorAffinity = (IntPtr)highCore;
                    }

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("GG'S FPS Booster v1.0 by GUGUDALI\n\n");
                    Console.WriteLine("CS2's FPS boosted.\n\n");
                    Console.WriteLine("Waiting for CS2 to close...");
                    fpsProcess.WaitForExit();

                    Console.Clear();
                    Console.WriteLine("GG'S FPS Booster v1.0 by GUGUDALI\n\n");
                    Console.WriteLine("CS2 closed. Restoring settings...");

                    for (int i = 0; i < processes.Length; i++)
                    {
                        try
                        {
                            proc = processes[i];
                                for (int j = 0; j < processList.Length; j++)
                                {
                                    try
                                    {
                                        if (proc.Id == processList[j].processID)
                                        {
                                            proc.PriorityClass = processList[j].processPriorityClass;
                                            proc.PriorityBoostEnabled = processList[j].priorityBoostEnabled;
                                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && proc.ProcessName != "svchost" && proc.ProcessName != "System" && proc.ProcessName != "Idle" && proc.ProcessName != "csrss" && proc.ProcessName != "smss" && proc.ProcessName != "Registry" && proc.ProcessName != "wininit" && proc.ProcessName != "winlogon" && proc.ProcessName != "lsass" && proc.ProcessName != "lsm" && proc.ProcessName != "cs2")
                                            {
                                                proc.ProcessorAffinity = processList[j].processorAffinity;
                                            }
                                        }
                                    }catch{}
                                }
                        }catch{}
                    }
                    Thread.Sleep(3000);
                    isCSFinded = false;
                    processList = null;
                }catch{}
            }
        }
    }
}