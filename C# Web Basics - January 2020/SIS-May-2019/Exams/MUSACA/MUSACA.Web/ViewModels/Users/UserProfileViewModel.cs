namespace MUSACA.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using MUSACA.Web.ViewModels.Orders;

    public class UserProfileViewModel
    {
        public UserProfileViewModel()
        {
            this.Orders = new List<OrderProfileViewModel>();
        }

        public ICollection<OrderProfileViewModel> Orders { get; set; }
    }
}
