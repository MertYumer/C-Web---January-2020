namespace SharedTrip.ViewModels
{
    using System.Collections.Generic;

    public class TripAllViewModel
    {
        public TripAllViewModel()
        {
            this.Trips = new List<TripViewModel>();
        }

        public ICollection<TripViewModel> Trips { get; set; }
    }
}
