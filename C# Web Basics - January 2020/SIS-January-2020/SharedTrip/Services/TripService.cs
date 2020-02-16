namespace SharedTrip.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using SharedTrip.BindingModels;
    using SharedTrip.Models;

    public class TripService : ITripService
    {
        private readonly ApplicationDbContext context;
        private readonly IUserService userService;

        public TripService(ApplicationDbContext context, IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public void AddTrip(TripAddBindingModel model)
        {
            var trip = new Trip
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                DepartureTime = DateTime.Parse(model.DepartureTime),
                Seats = int.Parse(model.Seats),
                Description = model.Description,
                ImagePath = WebUtility.UrlDecode(model.ImagePath)
            };

            this.context.Add(trip);
            this.context.SaveChanges();
        }

        public bool AddUserToTrip(string tripId, string userId)
        {
            var trip = this.GetTripById(tripId);
            var user = this.userService.GetUserById(userId);

            if (trip.Seats == 0)
            {
                return false;
            }

            var userTrip = new UserTrip
            {
                UserId = user.Id,
                TripId = trip.Id
            };

            trip.Seats--;
            this.context.UserTrips.Add(userTrip);
            this.context.SaveChanges();

            return true;
        }

        public bool CheckIfUserAlreadyJoined(string tripId, string userId)
        {
            var pair = this.context
                .UserTrips
                .SingleOrDefault(ut => ut.TripId == tripId && ut.UserId == userId);

            if (pair == null)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            var tripsFromDb = this.context.Trips.ToList();

            return tripsFromDb;
        }

        public Trip GetTripById(string tripId)
        {
            var tripFromDb = this.context.Trips.SingleOrDefault(t => t.Id == tripId);

            return tripFromDb;
        }
    }
}
