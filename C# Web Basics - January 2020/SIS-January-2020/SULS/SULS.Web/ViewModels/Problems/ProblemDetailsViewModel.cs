namespace SULS.Web.ViewModels.Problems
{
    using System.Collections.Generic;

    using SULS.Web.ViewModels.Submissions;

    public class ProblemDetailsViewModel
    {
        public ProblemDetailsViewModel()
        {
            this.Submissions = new List<SubmissionDetailsViewModel>();
        }

        public string Name { get; set; }

        public ICollection<SubmissionDetailsViewModel> Submissions { get; set; }
    }
}
