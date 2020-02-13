namespace IRunes.App.Controllers
{
    using IRunes.App.BindindModels.Tracks;
    using IRunes.App.ViewModels.Tracks;
    using IRunes.Services;
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class TracksController : Controller
    {
        private readonly IAlbumService albumService;
        private readonly ITrackService trackService;

        public TracksController(IAlbumService albumService, ITrackService trackService)
        {
            this.albumService = albumService;
            this.trackService = trackService;
        }

        public HttpResponse Create(string albumId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var trackCreateViewModel = new TrackCreateViewModel
            {
                AlbumId = albumId
            };

            return this.View(trackCreateViewModel);
        }

        [HttpPost]
        public HttpResponse Create(TrackCreateBindingModel model)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(model.Name) || 
                string.IsNullOrWhiteSpace(model.Link))
            {
                return this.Redirect($"/Tracks/Create?albumId={model.AlbumId}");
            }

            if (!this.albumService.AddTrackToAlbum(model.Name, model.Link, model.Price, model.AlbumId))
            {
                return this.Redirect("/Albums/All");
            }

            return this.Redirect($"/Albums/Details?albumId={model.AlbumId}");
        }

        public HttpResponse Details(TrackDetailsBindingModel model)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var albumFromDb = this.albumService.GetAlbumById(model.AlbumId);

            if (albumFromDb == null)
            {
                return this.Redirect("/Albums/All");
            }

            var trackFromDb = this.trackService.GetTrackById(model.TrackId);

            if (trackFromDb == null)
            {
                return this.Redirect($"/Albums/Details?albumId={model.AlbumId}");
            }

            var trackDetailsViewModel = new TrackDetailsViewModel
            {
                Name = trackFromDb.Name,
                Price = trackFromDb.Price,
                Link = trackFromDb.Link,
                AlbumId = model.AlbumId
            };

            return this.View(trackDetailsViewModel);
        }
    }
}
