namespace IRunes.App
{
    using System.Collections.Generic;

    using IRunes.Data;
    using IRunes.Services;
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class Startup : IMvcApplication
    {
        public void Configure(IList<Route> serverRoutingTable)
        {
            using (var db = new RunesDbContext())
            {
                db.Database.EnsureCreated();
            }
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUserService, UserService>();
            serviceCollection.Add<IAlbumService, AlbumService>();
            serviceCollection.Add<ITrackService, TrackService>();
        }
    }
}
