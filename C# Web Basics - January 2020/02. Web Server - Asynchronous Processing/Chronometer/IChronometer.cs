namespace Chronometer
{
    using System.Collections.Generic;

    public interface IChronometer
    {
        List<string> Laps { get; }

        string GetTime();

        void Start();

        void Stop();

        string Lap();

        void Reset();

        string GetLaps();
    }
}
