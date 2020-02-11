namespace SULS.Web.Controllers
{
    using System;

    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Result;
    using SULS.Models;
    using SULS.Services;
    using SULS.Web.BindingModels.Submissions;
    using SULS.Web.ViewModels.Problems;

    public class SubmissionsController : Controller
    {
        private readonly IProblemService problemService;
        private readonly ISubmissionService submissionService;

        public SubmissionsController(IProblemService problemService, ISubmissionService submissionService)
        {
            this.problemService = problemService;
            this.submissionService = submissionService;
        }

        [Authorize]
        public IActionResult Create(string problemId)
        {
            var problemFromDb = this.problemService.GetProblemById(problemId);

            if (problemFromDb == null)
            {
                return this.Redirect("/");
            }

            var problemSubmissionViewModel = new ProblemSubmissionViewModel
            {
                ProblemId = problemId,
                Name = problemFromDb.Name
            };

            return this.View(problemSubmissionViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(SubmissionCreateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect($"/Submissions/Create?problemId={model.ProblemId}");
            }

            var problemFromDb = this.problemService.GetProblemById(model.ProblemId);

            var random = new Random();
            var achievedResult = random.Next(0, problemFromDb.Points);

            var submission = new Submission
            {
                Code = model.Code,
                AchievedResult = achievedResult,
                CreatedOn = DateTime.UtcNow,
                ProblemId = model.ProblemId,
                UserId = model.UserId
            };

            this.submissionService.CreateSubmission(submission);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Delete(string submissionId)
        {
            this.submissionService.DeleteSubmission(submissionId);

            return this.Redirect("/");
        }
    }
}
