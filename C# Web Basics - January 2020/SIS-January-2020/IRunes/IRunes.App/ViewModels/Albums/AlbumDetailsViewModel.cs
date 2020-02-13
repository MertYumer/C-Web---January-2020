namespace IRunes.App.ViewModels.Albums
{
    using System.Collections.Generic;

    using IRunes.App.ViewModels.Tracks;

    public class AlbumDetailsViewModel
    {
        public AlbumDetailsViewModel()
        {
            this.Tracks = new List<TrackAlbumAllViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Cover { get; set; }

        public IList<TrackAlbumAllViewModel> Tracks { get; set; }
    }
}
