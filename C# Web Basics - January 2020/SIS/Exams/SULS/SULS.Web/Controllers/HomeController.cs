namespace SULS.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Result;
    using SULS.Services;
    using SULS.Web.ViewModels.Problems;

    public class HomeController : Controller
    {
        private readonly IProblemService problemService;
        public HomeController(IProblemService problemService)
        {
            this.problemService = problemService;
        }

        [HttpGet(Url = "/")]
        public IActionResult IndexSlash()
        {
            return Index();
        }

        public IActionResult Index()
        {
            var allProblemsViewModel = new List<ProblemHomeViewModel>();

            if (this.IsLoggedIn())
            {
                var problemsFromDb = this.problemService.GetAllProblems();

                allProblemsViewModel = problemsFromDb
                    .Select(p => new ProblemHomeViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Count = this.problemService.GetCountOfProblemSubmissions(p.Id)
                    })
                    .ToList();
            }

            return this.View(allProblemsViewModel);
        }
    }
}