namespace Chronometer
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class Chronometer : IChronometer
    {
        private long milliseconds;

        private bool isRunning;

        public Chronometer()
        {
            this.Reset();
        }

        public List<string> Laps { get; private set; }

        public string GetLaps()
        {
            if (this.Laps.Count == 0)
            {
                return "Laps: no laps";
            }

            var sb = new StringBuilder();
            sb.AppendLine("Laps: ");

            for (int i = 0; i < this.Laps.Count; i++)
            {
                sb.AppendLine($"{i}. {this.Laps[i]}");
            }

            return sb.ToString().TrimEnd();
        }

        public string GetTime()
            => $"{this.milliseconds / 60000:d2}:{this.milliseconds / 1000:d2}:{this.milliseconds % 1000:d4}";

        public string Lap()
        {
            var lap = GetTime();
            this.Laps.Add(lap);

            return lap;
        }

        public void Reset()
        {
            this.Stop();
            this.milliseconds = 0;
            this.Laps = new List<string>();
        }

        public void Start()
        {
            this.isRunning = true;

            Task.Run(() =>
            {
                while (this.isRunning)
                {
                    Thread.Sleep(1);
                    milliseconds++;
                }
            });
        }

        public void Stop()
        {
            this.isRunning = false;
        }
    }
}
