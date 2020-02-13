namespace PANDA.Web.ViewModels.Users
{
    using SIS.MvcFramework.Attributes.Validation;

    public class UserLoginInputModel
    {
        private const string UsernameErrorMessage = "Invalid username length! Must be between 5 and 20 symbols!";

        [RequiredSis]
        [StringLengthSis(5, 20, UsernameErrorMessage)]
        public string Username { get; set; }

        [RequiredSis]
        public string Password { get; set; }
    }
}
