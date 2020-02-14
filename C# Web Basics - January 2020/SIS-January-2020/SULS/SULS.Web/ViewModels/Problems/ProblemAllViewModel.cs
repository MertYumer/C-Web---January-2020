namespace SULS.Web.ViewModels.Problems
{
    using System.Collections.Generic;

    public class ProblemAllViewModel
    {
        public ProblemAllViewModel()
        {
            this.Problems = new List<ProblemHomeViewModel>();
        }

        public ICollection<ProblemHomeViewModel> Problems { get; set; }
    }
}
