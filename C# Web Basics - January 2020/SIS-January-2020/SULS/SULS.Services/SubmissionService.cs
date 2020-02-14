namespace SULS.Services
{
    using System;
    using System.Linq;

    using SULS.Data;
    using SULS.Models;

    public class SubmissionService : ISubmissionService
    {
        private readonly SulsDbContext context;
        private readonly IProblemService problemService;
        private readonly IUserService userService;

        public SubmissionService(SulsDbContext context, IProblemService problemService, IUserService userService)
        {
            this.context = context;
            this.problemService = problemService;
            this.userService = userService;
        }

        public void CreateSubmission(string code, string problemId, string userId)
        {
            var problemFromDb = this.problemService.GetProblemById(problemId);
            var userFromDb = this.userService.GetUserById(userId);

            var random = new Random();
            var achievedResult = random.Next(0, problemFromDb.Points);

            var submission = new Submission
            { 
                Code = code,
                AchievedResult = achievedResult,
                CreatedOn = DateTime.UtcNow,
                ProblemId = problemFromDb.Id,
                UserId = userFromDb.Id
            };

            this.context.Submissions.Add(submission);
            this.context.SaveChanges();
        }

        public void DeleteSubmission(string submissionId)
        {
            var submissionFromDb = this.context
                .Submissions
                .SingleOrDefault(s => s.Id == submissionId);

            this.context.Submissions.Remove(submissionFromDb);
            this.context.SaveChanges();
        }
    }
}
