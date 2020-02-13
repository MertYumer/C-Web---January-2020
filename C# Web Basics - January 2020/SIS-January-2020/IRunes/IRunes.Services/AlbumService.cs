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

        public AlbumService(RunesDbContext context)
        {
            this.context = context;
        }

        public bool AddTrackToAlbum(string name, string link, decimal price, string albumId)
        {
            var albumFromDb = this.GetAlbumById(albumId);

            if (albumFromDb == null)
            {
                return false;
            }

            var track = new Track
            {
                Name = name,
                Link = link,
                Price = price
            };

            albumFromDb.Tracks.Add(track);

            albumFromDb.Price = albumFromDb
                .Tracks
                .Select(t => t.Price)
                .Sum() * 0.87m;

            this.context.Update(albumFromDb);
            this.context.SaveChanges();

            return true;
        }

        public void CreateAlbum(string name, string cover)
        {
            var album = new Album
            {
                Name = name,
                Cover = cover,
                Price = 0.0m
            };

            this.context.Albums.Add(album);
            this.context.SaveChanges();
        }

        public Album GetAlbumById(string albumId)
        {
            var album = this.context
                .Albums
                .Include(a => a.Tracks)
                .SingleOrDefault(a => a.Id == albumId);

            return album;
        }

        public ICollection<Album> GetAllAlbums()
        {
            var allAlbums = context.Albums.ToList();

            return allAlbums;
        }
    }
}
