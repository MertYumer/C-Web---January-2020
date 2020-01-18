namespace IRunes.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IRunes.Data;
    using IRunes.Models;
    using Microsoft.EntityFrameworkCore;

    public class AlbumService : IAlbumService
    {
        private readonly RunesDbContext context;

        public AlbumService(RunesDbContext runesDbContext)
        {
            this.context = runesDbContext;
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

        public ICollection<Album> GetAllAlbums()
        {
            var allAlbums = context.Albums.ToList();
            Console.WriteLine(allAlbums.Count);
            return allAlbums;
        }

        public bool AddTrackToAlbum(string albumId, Track track)
        {
             var albumFromDb = context.Albums.FirstOrDefault(a => a.Id == albumId);

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
