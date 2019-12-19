namespace Demo.App
{
    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Routing;

    public class Launcher
    {
        public static void Main()
        {
            var serverRoutingTable = new ServerRoutingTable();

            //serverRoutingTable.Add(
            //    HttpRequestMethod.Get,
            //    "/",
            //    request => new HomeController().Index(request));

            var server = new Server(8000, serverRoutingTable);
            server.Run();
        }
    }
}
