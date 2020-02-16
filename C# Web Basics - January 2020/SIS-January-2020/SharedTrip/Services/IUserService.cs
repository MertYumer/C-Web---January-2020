namespace SharedTrip.Services
{
    using SharedTrip.BindindModels;
    using SharedTrip.Models;

    public interface IUserService
    {
        void Register(UserRegisterBindingModel model);

        User GetUser(string username, string password);

        User GetUserById(string userId);

        string GetUsername(string userId);

        bool UsernameExists(string username);

        bool EmailExists(string email);
    }
}
