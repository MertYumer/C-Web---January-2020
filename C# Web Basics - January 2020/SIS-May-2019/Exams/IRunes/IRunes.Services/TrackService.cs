namespace IRunes.Services
{
    using System.Linq;

    using IRunes.Data;
    using IRunes.Models;

    public class TrackService : ITrackService
    {
        private readonly RunesDbContext context;

        public TrackService(RunesDbContext context)
        {
            this.context = context;
        }

        public Track GetTrackById(string trackId)
        {
            var track = this.context
                .Tracks
                .FirstOrDefault(t => t.Id == trackId);

            return track;
        }
    }
}
