namespace SharedTrip.Services
{
    using System.Collections.Generic;

    using SharedTrip.BindingModels;
    using SharedTrip.Models;

    public interface ITripService
    {
        void AddTrip(TripAddBindingModel model);

        IEnumerable<Trip> GetAllTrips();

        Trip GetTripById(string tripId);

        bool CheckIfUserAlreadyJoined(string tripId, string userId);

        bool AddUserToTrip(string tripId, string userId);
    }
}
