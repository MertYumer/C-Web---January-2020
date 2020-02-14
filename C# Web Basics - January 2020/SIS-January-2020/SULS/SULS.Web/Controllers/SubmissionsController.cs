namespace SULS.Web.Controllers
{
    using SIS.HTTP;
    using SIS.MvcFramework;
    using SULS.Services;
    using SULS.Web.BindingModels;
    using SULS.Web.ViewModels.Submissions;

    public class SubmissionsController : Controller
    {
        private readonly ISubmissionService submissionService;
        private readonly IProblemService problemService;

        public SubmissionsController(ISubmissionService submissionService, IProblemService problemService)
        {
            this.submissionService = submissionService;
            this.problemService = problemService;
        }

        public HttpResponse Create(string problemId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var problemFromDb = this.problemService.GetProblemById(problemId);

            var viewModel = new ProblemSubmissionViewModel
            {
                ProblemId = problemFromDb.Id,
                ProblemName = problemFromDb.Name
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public HttpResponse Create(SubmissionCreatBindingModel model)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(model.Code))
            {
                return this.Redirect($"/Submissions/Create?problemId={model.ProblemId}");
            }

            this.submissionService.CreateSubmission(model.Code, model.ProblemId, model.User);

            return this.Redirect("/");
        }

        public HttpResponse Delete(string submissionId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.submissionService.DeleteSubmission(submissionId);

            return this.Redirect("/");
        }
    }
}
