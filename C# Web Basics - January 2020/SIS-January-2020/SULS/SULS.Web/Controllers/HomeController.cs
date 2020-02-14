namespace SULS.Web.Controllers
{
    using SIS.HTTP;
    using SIS.MvcFramework;
    using SULS.Services;
    using SULS.Web.ViewModels.Problems;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly IProblemService problemService;

        public HomeController(IProblemService problemService)
        {
            this.problemService = problemService;
        }

        [HttpGet("/")]
        public HttpResponse IndexSlash()
        {
            return Index();
        }

        public HttpResponse Index()
        {
            var problemAllViewModel = new ProblemAllViewModel();

            if (this.IsUserLoggedIn())
            {
                var problemsFromDb = this.problemService.GetAllProblems();

                problemAllViewModel.Problems = problemsFromDb
                    .Select(p => new ProblemHomeViewModel
                    { 
                        Id = p.Id,
                        Name = p.Name,
                        Count = this.problemService.GetAllProblemSubscriptionsCount(p.Id)
                    })
                    .ToList();
            }

            return this.View(problemAllViewModel);
        }
    }
}
