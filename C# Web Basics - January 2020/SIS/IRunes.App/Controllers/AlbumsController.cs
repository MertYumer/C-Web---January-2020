namespace IRunes.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using IRunes.App.ViewModels.Albums;
    using IRunes.Models;
    using IRunes.Services;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Mapping;
    using SIS.MvcFramework.Result;

    public class AlbumsController : Controller
    {
        private readonly IAlbumService albumService;

        public AlbumsController()
        {
            this.albumService = new AlbumService();
        }

        [Authorize]
        public IActionResult All()
        {
            ICollection<Album> allAlbums = this.albumService.GetAllAlbums();

            if (!allAlbums.Any())
            {
                this.ViewData["Albums"] = "There are currently no albums.";
            }

            else
            {
                this.ViewData["Albums"] =
                string.Join("<br/>",
                allAlbums
                .Select(a => $"<a class=\"text-primary font-weight-bold\" href=/Albums/Details?albumId={a.Id}>{WebUtility.UrlDecode(a.Name)}</a>")
                .ToList());
            }

            return this.View();
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost(ActionName = "Create")]
        public IActionResult CreateConfirm()
        {
            var name = ((ISet<string>)this.Request.FormData["name"]).FirstOrDefault();
            var cover = ((ISet<string>)this.Request.FormData["cover"]).FirstOrDefault();

            var album = new Album
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Cover = cover,
                Price = 0m
            };

            this.albumService.CreateAlbum(album);

            return this.Redirect("/Albums/All");
        }

        [Authorize]
        public IActionResult Details()
        {
            var albumId = this.Request.QueryData["albumId"].ToString();
            var albumFromDb = this.albumService.GetAlbumById(albumId);

            var albumDetailsViewModel = ModelMapper.ProjectTo<AlbumDetailsViewModel>(albumFromDb);

            if (albumFromDb == null)
            {
                return this.Redirect("/Albums/All");
            }

            this.ViewData["AlbumId"] = albumFromDb.Id;
            this.ViewData["AlbumName"] = WebUtility.UrlDecode(albumFromDb.Name);
            this.ViewData["AlbumCover"] = WebUtility.UrlDecode(albumFromDb.Cover);
            this.ViewData["AlbumPrice"] = $"${albumFromDb.Price:f2}";

            var tracks = albumFromDb.Tracks.ToList();
            var tracksHtml = string.Empty;

            if (!tracks.Any())
            {
                tracksHtml = "<p>Nothing to show...</p>" +
                             Environment.NewLine +
                             "<p>This album has no tracks added yet!</p>";
            }

            else
            {
                for (int i = 0; i < tracks.Count; i++)
                {
                    tracksHtml += $"<li>{i + 1}. <a class=\"text-primary font-weight-bold\" href=\"/Tracks/Details?albumId={tracks[i].AlbumId}&trackId={tracks[i].Id}\">" + WebUtility.UrlDecode(tracks[i].Name) + "</a></li>";
                }
            }

            this.ViewData["AlbumTracks"] = tracksHtml;

            return this.View(albumDetailsViewModel);
        }
    }
}
