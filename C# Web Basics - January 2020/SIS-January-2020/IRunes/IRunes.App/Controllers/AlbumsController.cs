namespace IRunes.App.Controllers
{
    using System.Linq;

    using IRunes.App.BindindModels.Albums;
    using IRunes.App.ViewModels.Albums;
    using IRunes.App.ViewModels.Tracks;
    using IRunes.Services;
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class AlbumsController : Controller
    {
        private readonly IAlbumService albumService;

        public AlbumsController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var albumAllViewModel = new AlbumAllViewModel
            {
                Albums = this.albumService
                .GetAllAlbums()
                .Select(a => new AlbumInfoViewModel
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToList()
            };

            return this.View(albumAllViewModel);
        }

        public HttpResponse Create()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(AlbumCreateBindingModel model)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(model.Name) ||
                string.IsNullOrWhiteSpace(model.Cover) ||
                model.Name.Length < 4 ||
                model.Name.Length > 20)
            {
                return this.Redirect("/Albums/Create");
            }

            this.albumService.CreateAlbum(model.Name, model.Cover);

            return this.Redirect("/Albums/All");
        }

        public HttpResponse Details(string albumId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var albumFromDb = this.albumService.GetAlbumById(albumId);

            if (albumFromDb == null)
            {
                return this.Redirect("/Albums/All");
            }

            var albumDetailsViewModel = new AlbumDetailsViewModel
            {
                Id = albumFromDb.Id,
                Name = albumFromDb.Name,
                Cover = albumFromDb.Cover,
                Price = albumFromDb.Price,
                Tracks = albumFromDb
                .Tracks
                .Select(t => new TrackAlbumAllViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToList()
            };

            return this.View(albumDetailsViewModel);
        }
    }
}
