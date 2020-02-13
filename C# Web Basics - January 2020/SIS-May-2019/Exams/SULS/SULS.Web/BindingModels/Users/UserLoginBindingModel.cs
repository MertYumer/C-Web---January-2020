namespace SULS.Web.BindingModels.Users
{
    using SIS.MvcFramework.Attributes.Validation;

    public class UserLoginBindingModel
    {
        private const string UsernameErrorMessage = "Invalid username length! Must be between 5 and 20 symbols!";

        private const string PasswordErrorMessage = "Invalid password length! Must be between 6 and 20 symbols!";

        [RequiredSis]
        [StringLengthSis(5, 20, UsernameErrorMessage)]
        public string Username { get; set; }

        [RequiredSis]
        [StringLengthSis(6, 20, PasswordErrorMessage)]
        public string Password { get; set; }
    }
}
