namespace MUSACA.Web.BindingModels.Users
{
    using SIS.MvcFramework.Attributes.Validation;

    public class UserRegisterBindingModel
    {
        private const string UsernameErrorMessage = "Invalid username length! Must be between 5 and 20 symbols!";

        private const string PasswordErrorMessage = "Invalid password length!";

        private const string EmailErrorMessage = "Invalid email!";

        [RequiredSis]
        [StringLengthSis(5, 20, UsernameErrorMessage)]
        public string Username { get; set; }

        [RequiredSis]
        [PasswordSis(PasswordErrorMessage)]
        public string Password { get; set; }

        [RequiredSis]
        [PasswordSis(PasswordErrorMessage)]
        public string ConfirmPassword { get; set; }

        [RequiredSis]
        [EmailSis(EmailErrorMessage)]
        [StringLengthSis(5, 20, UsernameErrorMessage)]
        public string Email { get; set; }
    }
}
