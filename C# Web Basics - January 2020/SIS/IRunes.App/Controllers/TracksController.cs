namespace IRunes.App.Controllers
{
    using System;

    using IRunes.App.ViewModels.Tracks;
    using IRunes.Models;
    using IRunes.Services;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Mapping;
    using SIS.MvcFramework.Result;

    public class TracksController : Controller
    {
        private readonly ITrackService trackService;

        private readonly IAlbumService albumService;

        public TracksController(ITrackService trackService, IAlbumService albumService)
        {
            this.trackService = trackService;
            this.albumService = albumService;
        }

        [Authorize]
        public IActionResult Create(string albumId)
        {
            var trackCreateViewModel = new TrackCreateViewModel{ AlbumId = albumId };

            return this.View(trackCreateViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(TrackCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Albums/All");
            }

            var track = ModelMapper.ProjectTo<Track>(model);

            if (!this.albumService.AddTrackToAlbum(model.AlbumId, track))
            {
                return this.Redirect("/Albums/All");
            }

            return this.Redirect($"/Albums/Details?id={model.AlbumId}");
        }

        [Authorize]
        public IActionResult Details(TrackDetailsInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect($"Albums/All");
            }

            var albumFromDb = this.albumService.GetAlbumById(model.AlbumId);
            var trackFromDb = this.trackService.GetTrackById(model.TrackId);

            if (albumFromDb == null)
            {
                return this.Redirect("/Albums/All");
            }

            else if (trackFromDb == null)
            {
                return this.Redirect($"/Albums/Details?id={albumFromDb.Id}");
            }

            TrackDetailsViewModel trackDetailsViewModel = ModelMapper.ProjectTo<TrackDetailsViewModel>(trackFromDb);
            trackDetailsViewModel.AlbumId = model.AlbumId;

            return this.View(trackDetailsViewModel);
        }
    }
}
