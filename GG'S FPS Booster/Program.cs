// Version: 1.1
// Description: GG'S FPS Booster is a program that allows you to boost your CS2 FPS by changing the priority of the game process and setting the processor affinity.
// Developer: GUGUDALI
// GitHub: https://github.com/GUGUDALI/GG-S-FPS-Booster
// Last update: 08/01/2025
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GG_S_FPS_Booster
{
    class ProcessList
    {
        public int ProcessID { get; set; }
        public ProcessPriorityClass ProcessPriorityClass { get; set; }
        public bool PriorityBoostEnabled { get; set; }
        public nint ProcessorAffinity { get; set; }
    }

    class Program
    {
        static int idleCore;
        static int highCore;
        static Process[]? processes;
        static readonly int[] cores = new int[Environment.ProcessorCount];
        static Process? csProcess;
        static Process? process;
        static ProcessList[]? processList;
        static bool iscsFinded = false;
        static readonly string[] exList = {"cs2", "csrss", "Idle", "Registry", "SecurityHealthService", "services", "SgrmBroker", "smss", "System", "wininit"};
        static readonly string title = "GG'S FPS Booster v1.1 by GUGUDALI";
        static readonly bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        static void Main(string[] args)
        {
            try
            {
                InitializeApp();
                while (true)
                {
                    WaitForCS();
                    Thread.Sleep(500);
                    BoostCS();
                    Thread.Sleep(500);
                    WaitCSClose();
                    Thread.Sleep(500);
                    RestoreSettings();
                    Thread.Sleep(3000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Restore the processor affinity and priority of the processes
        static void RestoreSettings()
        {
            try
            {
                if (processList != null)
                {
                    WriteConsole("Restoring Settings...", ConsoleColor.Yellow);
                    processes = Process.GetProcesses();
                    foreach (var process in processes)
                    {
                        try
                        {
                            foreach (var exProcess in processList){
                                try
                                {
                                    if (exProcess.ProcessID==process.Id){
                                        process.PriorityClass = exProcess.ProcessPriorityClass;
                                        process.PriorityBoostEnabled = exProcess.PriorityBoostEnabled;
                                        if (!exList.Contains(process.ProcessName))
                                        {
                                            process.ProcessorAffinity = exProcess.ProcessorAffinity;
                                        }
                                    }
                                }catch{}
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    csProcess = null;
                    processList = null;
                    iscsFinded = false;
                    WriteConsole("Settings restored.", ConsoleColor.Yellow);
                    Thread.Sleep(3000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Wait for the CS2 process to close
        static void WaitCSClose()
        {
            if (csProcess != null)
            {
                csProcess.WaitForExit();
                WriteConsole("CS2 closed.\n\nResetting Settings...", ConsoleColor.White);
            }
            else iscsFinded = false;
        }

        // Boost the CS2 process
        static void BoostCS()
        {
            try
            {
                if (csProcess != null)
                {
                    WriteConsole("Boosting CS2's FPS...", ConsoleColor.Green);

                    processes = Process.GetProcesses();
                    processList = new ProcessList[processes.Length];

                    for (int i = 0; i < processes.Length; i++)
                    {
                        try
                        {
                            process = processes[i];
                            try
                            {
                                processList[i] = new ProcessList
                                {
                                    ProcessID = process.Id,
                                    ProcessPriorityClass = process.PriorityClass,
                                    PriorityBoostEnabled = process.PriorityBoostEnabled,
                                    ProcessorAffinity = process.ProcessorAffinity
                                };
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            Console.WriteLine($"{processList[i].ProcessID.ToString()} {processList[i].ProcessPriorityClass.ToString()} {processList[i].PriorityBoostEnabled.ToString()} {processList[i].ProcessorAffinity.ToString()}");
                            try
                            {
                                process.PriorityClass = ProcessPriorityClass.Idle;
                                process.PriorityBoostEnabled = false;
                                if (process.ProcessName != "csrss" && process.ProcessName != "Idle" && process.ProcessName != "Registry" && process.ProcessName != "SecurityHealthService" && process.ProcessName != "services" && process.ProcessName != "SgrmBroker" && process.ProcessName != "smss" && process.ProcessName != "System" && process.ProcessName != "wininit" && process.ProcessName != "cs2")
                                {
                                    process.ProcessorAffinity = (IntPtr)idleCore;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    };
                    Console.ReadLine();
                    csProcess.PriorityClass = ProcessPriorityClass.RealTime;
                    csProcess.PriorityBoostEnabled = true;
                    csProcess.ProcessorAffinity = (IntPtr)highCore;

                    WriteConsole("CS2's FPS boosted.\n\nWaiting for CS2 to close...", ConsoleColor.Green);
                }
                else
                {
                    iscsFinded = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Wait for the CS2 process
        static void WaitForCS()
        {
            try
            {
                while (!iscsFinded)
                {
                    WriteConsole("Waiting for CS2...", ConsoleColor.Cyan);

                    processes = Process.GetProcesses();
                    csProcess = processes.FirstOrDefault(p => p.ProcessName == "cs2");
                    iscsFinded = csProcess != null;

                    Thread.Sleep(3000);
                }
                if (csProcess != null)
                {
                    WriteConsole($"CS2 found! PID: {csProcess.Id}", ConsoleColor.Cyan);
                }
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Initialize the application
        static void InitializeApp()
        {
            try
            {
                AppDomain.CurrentDomain.ProcessExit += OnClosing;
                CheckWindows();
                Console.Title = title;
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
                if (cores.Length < 5)
                {
                    for (int i = 0; i < cores.Length; i++)
                    {
                        if (i == 0 || i == (cores.Length / 2))
                        {

                        }
                        else
                        {
                            idleCore += cores[i];
                        }
                    }
                }
                else
                {
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
                }
                for (int i = 0; i < cores.Length; i++)
                {
                    highCore += cores[i];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Reset the processor affinity and priority of the processes on closing
        static void OnClosing(object? sender, EventArgs e)
        {
            try
            {
                RestoreSettings();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Check if the program is running on Windows
        static void CheckWindows()
        {
            try
            {
                if (!isWindows)
                {
                    WriteConsole("This program is only available for Windows.\nPress any key to exit...", ConsoleColor.Red);
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Write the text to the console
        static void WriteConsole(string text, ConsoleColor color)
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = color;
                Console.WriteLine($"{title}\n\n{text}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}