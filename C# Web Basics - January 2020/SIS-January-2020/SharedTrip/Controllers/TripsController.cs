namespace SharedTrip.Controllers
{
    using System.Linq;

    using SharedTrip.BindingModels;
    using SharedTrip.Services;
    using SharedTrip.ViewModels;
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class TripsController : Controller
    {
        private readonly ITripService tripService;

        public TripsController(ITripService tripService)
        {
            this.tripService = tripService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var tripsFromDb = this.tripService.GetAllTrips();

            var tripAllViewModel = new TripAllViewModel
            {
                Trips = tripsFromDb.Select(t => new TripViewModel
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                    Seats = t.Seats
                })
                .ToList()
            };

            return this.View(tripAllViewModel);
        }

        public HttpResponse Add()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(TripAddBindingModel model)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(model.StartPoint) ||
                string.IsNullOrWhiteSpace(model.EndPoint) ||
                string.IsNullOrWhiteSpace(model.DepartureTime) ||
                string.IsNullOrWhiteSpace(model.Description) ||
                string.IsNullOrWhiteSpace(model.ImagePath) || 
                string.IsNullOrWhiteSpace(model.Seats) ||
                model.DepartureTime == null)
            {
                return this.View();
            }

            if (int.Parse(model.Seats) < 2 || int.Parse(model.Seats) > 6)
            {
                return this.View();
            }

            if (model.Description.Length > 80)
            {
                return this.View();
            }

            this.tripService.AddTrip(model);

            return this.Redirect("/Trips/All");
        }

        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var tripFromDb = this.tripService.GetTripById(tripId);

            if (tripFromDb == null)
            {
                return this.Redirect("/Trips/All");
            }

            var tripDetailsViewModel = new TripDetailsViewModel
            {
                Id = tripFromDb.Id,
                StartPoint = tripFromDb.StartPoint,
                EndPoint = tripFromDb.EndPoint,
                DepartureTime = tripFromDb.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                Seats = tripFromDb.Seats,
                Description = tripFromDb.Description,
                ImagePath = tripFromDb.ImagePath
            };

            return this.View(tripDetailsViewModel);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var tripFromDb = this.tripService.GetTripById(tripId);

            if (tripFromDb == null)
            {
                return this.Redirect("/Trips/All");
            }

            var userAlreadyJoined = this.tripService.CheckIfUserAlreadyJoined(tripId, this.User);

            if (userAlreadyJoined)
            {
                return this.Redirect($"/Trips/Details?tripId={tripId}");
            }

            var successfullyAddedUserToTrip = this.tripService.AddUserToTrip(tripId, this.User);

            if (!successfullyAddedUserToTrip)
            {
                return this.Redirect($"/Trips/Details?tripId={tripId}");
            }

            return this.Redirect("/Trips/All");
        }
    }
}
