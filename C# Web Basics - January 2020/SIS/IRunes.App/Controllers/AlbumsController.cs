namespace IRunes.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public AlbumsController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [Authorize]
        public IActionResult All()
        {
            ICollection<Album> allAlbums = this.albumService.GetAllAlbums();

            if (allAlbums.Count > 0)
            {
                return this.View(allAlbums.Select(ModelMapper.ProjectTo<AlbumAllViewModel>).ToList());
            }

            return this.View(new List<AlbumAllViewModel>());
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(AlbumCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Albums/Create");
            }

            var album = ModelMapper.ProjectTo<Album>(model);
            this.albumService.CreateAlbum(album);

            return this.Redirect("/Albums/All");
        }

        [Authorize]
        public IActionResult Details(AlbumDetailsInputModel model)
        {
            var albumFromDb = this.albumService.GetAlbumById(model.Id);
            var albumDetailsViewModel = ModelMapper.ProjectTo<AlbumDetailsViewModel>(albumFromDb);

            if (albumFromDb == null)
            {
                return this.Redirect("/Albums/All");
            }

            return this.View(albumDetailsViewModel);
        }
    }
}
