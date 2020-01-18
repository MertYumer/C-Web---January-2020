namespace IRunes.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using IRunes.Data;
    using IRunes.Models;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Result;

    public class TracksController : Controller
    {
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

            using (var context = new RunesDbContext())
            {
                var albumFromDb = context.Albums.FirstOrDefault(a => a.Id == albumId);

                if (albumFromDb == null)
                {
                    return this.Redirect("/Albums/All");
                }

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

                albumFromDb.Tracks.Add(track);

                albumFromDb.Price = (albumFromDb
                .Tracks
                .Select(t => t.Price)
                .Sum() * 87) / 100;

                context.Update(albumFromDb);
                context.SaveChanges();

                return this.Redirect($"/Albums/Details?albumId={albumId}");
            }
        }

        [Authorize]
        public IActionResult Details()
        {
            using (var context = new RunesDbContext())
            {
                var albumId = this.Request.QueryData["albumId"].ToString();
                var trackId = this.Request.QueryData["trackId"].ToString();

                var albumFromDb = context.Albums.FirstOrDefault(x => x.Id == albumId);
                var trackFromDb = context.Tracks.FirstOrDefault(x => x.Id == trackId);

                if (albumFromDb == null || trackFromDb == null)
                {
                    return this.Redirect("/Albums/All");
                }

                this.ViewData["TrackName"] = WebUtility.UrlDecode(trackFromDb.Name);
                this.ViewData["TrackLink"] = WebUtility.UrlDecode(trackFromDb.Link);
                this.ViewData["TrackPrice"] = $"${trackFromDb.Price:f2}";
                this.ViewData["AlbumId"] = albumFromDb.Id;

                return this.View();
            }
        }
    }
}
