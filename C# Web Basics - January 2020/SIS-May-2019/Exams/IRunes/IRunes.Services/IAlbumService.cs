namespace IRunes.Services
{
    using System.Collections.Generic;

    using IRunes.Models;

    public interface IAlbumService
    {
        Album CreateAlbum(Album album);

        ICollection<Album> GetAllAlbums();

        bool AddTrackToAlbum(string albumId, Track track);

        Album GetAlbumById(string id);
    }
}
