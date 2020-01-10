namespace Demo.App
{
    using Demo.App.Controllers;
    using SIS.HTTP.Enums;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Routing;
    using SIS.MvcFramework.Routing.Contracts;

    public class Launcher
    {
        public static void Main()
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Add(
                HttpRequestMethod.Get,
                "/",
                request => new HomeController().Home(request));

            var server = new Server(8000, serverRoutingTable);
            server.Run();
        }
    }
}
