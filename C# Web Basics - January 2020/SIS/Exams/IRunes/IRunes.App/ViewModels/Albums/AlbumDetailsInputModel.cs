namespace IRunes.App.ViewModels.Albums
{
    using SIS.MvcFramework.Attributes.Validation;

    public class AlbumDetailsInputModel
    {
        [RequiredSis]
        public string Id { get; set; }
    }
}
