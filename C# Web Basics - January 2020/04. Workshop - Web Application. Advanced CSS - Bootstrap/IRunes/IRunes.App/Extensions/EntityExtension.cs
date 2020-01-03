namespace IRunes.App.Extensions
{
    using IRunes.Models;

    public static class EntityExtension
    {
        public static string ToHtmlAll(this Album album)
        {
            return $"<div><a href=/Albums/Details?id={album.Id}>{album.Name}</a></div>";
        }

        public static string ToHtmlDetails(this Album album)
        {
            return null;
        }

        public static string ToHtmlAll(this Track track)
        {
            return null;
        }

        public static string ToHtmlDetails(this Track track)
        {
            return null;
        }
    }
}
