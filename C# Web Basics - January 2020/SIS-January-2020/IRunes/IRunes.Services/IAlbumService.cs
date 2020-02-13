namespace IRunes.Services
{
    using System.Collections.Generic;

    using IRunes.Models;

    public interface IAlbumService
    {
        ICollection<Album> GetAllAlbums();

        void CreateAlbum(string name, string cover);

        Album GetAlbumById(string albumId);

        bool AddTrackToAlbum(string name, string link, decimal price, string albumId);
    }
}
