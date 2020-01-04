namespace IRunes.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using IRunes.Data;
    using IRunes.Models;
    using Microsoft.EntityFrameworkCore;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;

    public class TracksController : BaseController
    {
        public IHttpResponse Create(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }

            var albumId = httpRequest.QueryData["albumId"].ToString();
            ViewData["AlbumId"] = albumId;

            return this.View();
        }

        public IHttpResponse CreateConfirm(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }

            var albumId = httpRequest.QueryData["albumId"].ToString();

            using (var context = new RunesDbContext())
            {
                var albumFromDb = context.Albums.FirstOrDefault(a => a.Id == albumId);

                if (albumFromDb == null)
                {
                    return this.Redirect("/Albums/All");
                }

                var name = ((ISet<string>)httpRequest.FormData["name"]).FirstOrDefault();
                var link = ((ISet<string>)httpRequest.FormData["link"]).FirstOrDefault();
                var price = ((ISet<string>)httpRequest.FormData["price"]).FirstOrDefault();

                var track = new Track
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Link = link,
                    Price = decimal.Parse(price),
                    AlbumId = albumId
                };

                if (!this.IsValid(track))
                {
                    return this.Redirect($"/Albums/Details?albumId={albumId}");
                }

                albumFromDb.Tracks.Add(track);
                albumFromDb.Price = albumFromDb
                    .Tracks
                    .Sum(t => t.Price) * 0.87m;

                context.SaveChanges();

                return this.Redirect($"/Albums/Details?albumId={albumId}");
            }
        }

        public IHttpResponse Details(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }

            using (var context = new RunesDbContext())
            {
                var albumId = httpRequest.QueryData["albumId"].ToString();
                var trackId = httpRequest.QueryData["trackId"].ToString();

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
