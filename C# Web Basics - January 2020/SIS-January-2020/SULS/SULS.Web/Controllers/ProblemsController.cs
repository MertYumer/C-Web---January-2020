namespace SULS.Web.Controllers
{
    using SIS.HTTP;
    using SIS.MvcFramework;
    using SULS.Services;
    using SULS.Web.BindingModels.Problems;
    using SULS.Web.ViewModels.Problems;
    using SULS.Web.ViewModels.Submissions;
    using System.Linq;

    public class ProblemsController : Controller
    {
        private readonly IProblemService problemService;

        public ProblemsController(IProblemService problemService)
        {
            this.problemService = problemService;
        }

        public HttpResponse Create()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(ProblemCreateBindingModel model)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(model.Name) ||
                model.Points < 50 ||
                model.Points > 300)
            {
                return this.Redirect("/Problems/Create");
            }

            this.problemService.CreateProblem(model.Name, model.Points);

            return this.Redirect("/");
        }

        public HttpResponse Details(string problemId)
        {
            var problemFromDb = this.problemService.GetProblemById(problemId);
            var submissionFromDb = this.problemService.GetAllProblemSubmissions(problemId);

            var problemDetailsViewModel = new ProblemDetailsViewModel
            {
                Name = problemFromDb.Name,
                Submissions = submissionFromDb
                .Select(s => new SubmissionDetailsViewModel
                {
                    Id = s.Id,
                    Username = s.User.Username,
                    AchievedResult = $"{s.AchievedResult} / {problemFromDb.Points}",
                    CreatedOn = s.CreatedOn.ToString("dd/MM/yyyy")
                })
                .ToList()
            };

            return this.View(problemDetailsViewModel);
        }
    }
}
