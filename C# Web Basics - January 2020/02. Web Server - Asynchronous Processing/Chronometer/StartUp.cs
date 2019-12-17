namespace Chronometer
{
    using System;

    public class StartUp
    {
        public static void Main()
        {
            IChronometer chronometer = new Chronometer();
            string command;

            while (true)
            {
                command = Console.ReadLine();

                switch (command)
                {
                    case "exit": Environment.Exit(0); break;
                    case "start": chronometer.Start(); break;
                    case "stop": chronometer.Stop(); break;
                    case "lap": Console.WriteLine(chronometer.Lap()); break;
                    case "laps": Console.WriteLine(chronometer.GetLaps()); break;
                    case "time": Console.WriteLine(chronometer.GetTime()); break;
                    case "reset": chronometer.Reset(); break;
                };
            }
        }
    }
}
