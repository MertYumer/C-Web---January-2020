namespace IRunes.Services
{
    using System.Linq;

    using IRunes.Data;
    using IRunes.Models;

    public class TrackService : ITrackService
    {
        private readonly RunesDbContext context;

        public TrackService(RunesDbContext runesDbContext)
        {
            this.context = runesDbContext;
        }

        public Track GetTrackById(string trackId)
        {
            var track =  this.context
                .Tracks
                .FirstOrDefault(t => t.Id == trackId);

            return track;
        }
    }
}
