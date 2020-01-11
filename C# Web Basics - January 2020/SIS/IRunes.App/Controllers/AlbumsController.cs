namespace IRunes.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using IRunes.Data;
    using IRunes.Models;
    using Microsoft.EntityFrameworkCore;
    using SIS.HTTP.Requests;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes;

    public class AlbumsController : Controller
    {
        public IHttpResponse All(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }

            using (var context = new RunesDbContext())
            {
                if (!context.Albums.Any())
                {
                    this.ViewData["Albums"] = "There are currently no albums.";
                }

                else
                {
                    this.ViewData["Albums"] =
                    string.Join("<br/>",
                    context
                    .Albums
                    .Select(a => $"<a class=\"text-primary font-weight-bold\" href=/Albums/Details?albumId={a.Id}>{WebUtility.UrlDecode(a.Name)}</a>")
                    .ToList());
                }

                return this.View();
            }
        }

        public IHttpResponse Create(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public IHttpResponse CreateConfirm(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }

            using (var context = new RunesDbContext())
            {
                var name = ((ISet<string>)httpRequest.FormData["name"]).FirstOrDefault();
                var cover = ((ISet<string>)httpRequest.FormData["cover"]).FirstOrDefault();

                var album = new Album
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Cover = cover,
                    Price = 0m
                };

                //if (!this.IsValid(album))
                //{
                //    return this.Redirect("/Albums/Create");
                //}

                context.Albums.Add(album);
                context.SaveChanges();

                return this.Redirect("/Albums/All");
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
                var albumFromDb = context
                    .Albums
                    .Include(a => a.Tracks)
                    .FirstOrDefault(a => a.Id == albumId);

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

                return this.View();
            }
        }
    }
}
