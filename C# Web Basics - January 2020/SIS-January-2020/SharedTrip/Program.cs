namespace SharedTrip
{
    using System.Threading.Tasks;

    using SIS.MvcFramework;

    public static class Program
    {
        public static async Task Main()
        {
            //Changed the port in WebHost to 8000, because it gives me error on port 80
            await WebHost.StartAsync(new Startup());
        }
    }
}
