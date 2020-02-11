namespace SULS.Web.Controllers
{
    using System.Linq;

    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Mapping;
    using SIS.MvcFramework.Result;
    using SULS.Models;
    using SULS.Services;
    using SULS.Web.BindingModels.Problems;
    using SULS.Web.ViewModels.Problems;

    public class ProblemsController : Controller
    {
        private readonly IProblemService problemService;
        public ProblemsController(IProblemService problemService)
        {
            this.problemService = problemService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(ProblemCreateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Problems/Create");
            }

            var problem = ModelMapper.ProjectTo<Problem>(model);
            this.problemService.CreateProblem(problem);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Details(string problemId)
        {
            var problemFromDb = this.problemService.GetProblemById(problemId);

            if (problemFromDb == null)
            {
                return this.Redirect("/");
            }

            var submissionsFromDb = this.problemService.GetAllProblemSubmissions(problemId);

            var problemDetailsViewModel = new ProblemDetailsViewModel
            {
                Name = problemFromDb.Name,
                Submissions = submissionsFromDb
                .Select(s => new ViewModels.Submissions.SubmissionDetailsViewModel
                {
                    Id = s.Id,
                    Username = s.User.Username,
                    AchievedResult = s.AchievedResult.ToString(),
                    CreatedOn = s.CreatedOn.ToString("dd/MM/yyyy"),
                    MaxPoints = s.Problem.Points.ToString()
                })
                .ToList()
            };

            return this.View(problemDetailsViewModel);
        }
    }
}
