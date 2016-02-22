using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Speech.Synthesis;

namespace SystemMonitor
{
    class Program
    {
        /// <summary>
        /// Pulls information from diagnostics to tell user about their CPU usage and memory monitor. 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            #region Jarvis Voice
            //Make text to speech synthesizer to greet user
            SpeechSynthesizer Jarvis = new SpeechSynthesizer();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Hello, I am Jarvis. I like to tell you about your system diagnostics!");
            Jarvis.Speak("Hello, I am Jarvis. I like to tell you about your system diagnostics!");
            #endregion

            #region PerformanceCounters

            

            //Make a performance counter to gather CPU information
            PerformanceCounter perfomanceCPU = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");

            //Shows the amount of of available memory
            PerformanceCounter perfomanceMemory = new PerformanceCounter("Memory", "Available MBytes");

            //Make an uptime counter
            PerformanceCounter performanceUptime = new PerformanceCounter("System", "System Up Time");
            performanceUptime.NextValue();


            TimeSpan uptimeSpan = TimeSpan.FromSeconds(performanceUptime.NextValue());

            //Make the vocal message for the uptime
            string systemDuration = String.Format("The system has been turned on for {0} days {1} hours {2} minutes {3} seconds.", (int)uptimeSpan.TotalDays, (int)uptimeSpan.Hours, (int)uptimeSpan.Minutes, (int)uptimeSpan.Seconds);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(systemDuration);
            Jarvis.Speak(systemDuration);
            #endregion



            //change the color once again
            Console.ForegroundColor = ConsoleColor.Yellow;

            while (true)
            {

                //make variable to hold speech and string information about cpu
                int currentCPU = (int)perfomanceCPU.NextValue();
                
                //make variable to hold speech and string information about memory
                int currentMem = (int)perfomanceMemory.NextValue();


                //Will print CPU load percentage to the console
                Console.WriteLine("CPU load: {0}%", currentCPU);
                Jarvis.Speak(currentCPU.ToString());
                
                //Output the available memory
                Console.WriteLine("Available Memory: {0} MB", currentMem);
                

                //Make Jarvis alert you when your system is having CPU usage > 75% and/or Memory is less than 1 gig (1024 mb)
                if(currentCPU > 75)
                {
                    if (currentCPU == 99)
                    {
                        string vocalCPUMessage = String.Format("WARNING! WARNING! The CPU IS FRYING!");
                        Jarvis.Speak(vocalCPUMessage);
                    }

                    else
                    { 
                        string vocalCPUMessage = String.Format("The CPU load is: {0} percent", currentCPU);
                        Jarvis.Speak(vocalCPUMessage);
                    }
                }
                if (currentMem < 1024)
                {
                    string vocalMemMessage = String.Format("You have " + currentMem + "megabytes of memory free");
                    Jarvis.Speak(vocalMemMessage);
                }


                //End the loop
                Thread.Sleep(1000);

            }
        }
    }
}
