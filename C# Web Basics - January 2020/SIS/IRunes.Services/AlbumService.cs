namespace IRunes.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using IRunes.Data;
    using IRunes.Models;
    using Microsoft.EntityFrameworkCore;

    public class AlbumService : IAlbumService
    {
        private readonly RunesDbContext context;

        public AlbumService()
        {
            this.context = new RunesDbContext();
        }

        public ICollection<Album> GetAllAlbums()
        {
            var allAlbums = context.Albums.ToList();
            return allAlbums;
        }

        public Album CreateAlbum(Album album)
        {
            album = context.Albums.Add(album).Entity;
            context.SaveChanges();

            return album;
        }

        public Album GetAlbumById(string albumId)
        {
            var album = context
                .Albums
                .Include(a => a.Tracks)
                .FirstOrDefault(a => a.Id == albumId);

            return album;
        }

        public bool AddTrackToAlbum(string albumId, Track track)
        {
            var albumFromDb = this.GetAlbumById(albumId);

            if (albumFromDb == null)
            {
                return false;
            }

            albumFromDb.Tracks.Add(track);

            albumFromDb.Price = (albumFromDb
                .Tracks
                .Select(t => t.Price)
                .Sum() * 87) / 100;

            context.Update(albumFromDb);
            context.SaveChanges();

            return true;
        }
    }
}
