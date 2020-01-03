namespace IRunes.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using IRunes.App.Extensions;
    using IRunes.Data;
    using IRunes.Models;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;

    public class AlbumsController : BaseController
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
                    .Select(a => a.ToHtmlAll())
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

                if (!this.IsValid(album))
                {
                    return this.Redirect("/Albums/Create");
                }

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
                var albumId = httpRequest.QueryData["id"].ToString();
                var albumFromDb = context.Albums.FirstOrDefault(a => a.Id == albumId);

                if (albumFromDb == null)
                {
                    return this.Redirect("/Albums/All");
                }

                this.ViewData["AlbumId"] = albumFromDb.Id;
                this.ViewData["AlbumName"] = albumFromDb.Name;
                this.ViewData["AlbumCover"] = albumFromDb.Cover;
                this.ViewData["AlbumPrice"] = $"${albumFromDb.Price:f2}";

                return this.View();
            }
        }
    }
}
