namespace IRunes.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using IRunes.Models;
    using IRunes.Services;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Result;

    public class TracksController : Controller
    {
        private readonly ITrackService trackService;

        private readonly IAlbumService albumService;

        public TracksController()
        {
            this.trackService = new TrackService();
            this.albumService = new AlbumService();
        }

        [Authorize]
        public IActionResult Create()
        {
            var albumId = this.Request.QueryData["albumId"].ToString();
            ViewData["AlbumId"] = albumId;

            return this.View();
        }

        [Authorize]
        [HttpPost(ActionName = "Create")]
        public IActionResult CreateConfirm()
        {
            var albumId = this.Request.QueryData["albumId"].ToString();

            string name = ((ISet<string>)this.Request.FormData["name"]).FirstOrDefault();
            string link = ((ISet<string>)this.Request.FormData["link"]).FirstOrDefault();

            if (!decimal.TryParse(((ISet<string>)this.Request.FormData["price"]).FirstOrDefault(), out decimal price))
            {
                return this.Redirect($"/Tracks/Create?albumId={albumId}");
            }

            var track = new Track
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Link = link,
                Price = price,
                AlbumId = albumId
            };

            if (!this.albumService.AddTrackToAlbum(albumId, track))
            {
                return this.Redirect("/Albums/All");
            }

            return this.Redirect($"/Albums/Details?albumId={albumId}");
        }

        [Authorize]
        public IActionResult Details()
        {
            var albumId = this.Request.QueryData["albumId"].ToString();
            var trackId = this.Request.QueryData["trackId"].ToString();

            var albumFromDb = this.albumService.GetAlbumById(albumId);
            var trackFromDb = this.trackService.GetTrackById(trackId);

            if (albumFromDb == null)
            {
                return this.Redirect("/Albums/All");
            }

            else if (trackFromDb == null)
            {
                return this.Redirect($"/Albums/Details?id={albumFromDb.Id}");
            }

            this.ViewData["TrackName"] = WebUtility.UrlDecode(trackFromDb.Name);
            this.ViewData["TrackLink"] = WebUtility.UrlDecode(trackFromDb.Link);
            this.ViewData["TrackPrice"] = $"${trackFromDb.Price:f2}";
            this.ViewData["AlbumId"] = albumFromDb.Id;

            return this.View();
        }
    }
}
