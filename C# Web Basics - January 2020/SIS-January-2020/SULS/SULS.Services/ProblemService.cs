namespace SULS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SULS.Data;
    using SULS.Models;

    public class ProblemService : IProblemService
    {
        private readonly SulsDbContext context;

        public ProblemService(SulsDbContext context)
        {
            this.context = context;
        }

        public void CreateProblem(string name, int points)
        {
            var problem = new Problem
            {
                Name = name,
                Points = points
            };

            this.context.Problems.Add(problem);
            this.context.SaveChanges();
        }

        public ICollection<Problem> GetAllProblems()
        {
            var problemsFromDb = this.context
                .Problems
                .ToList();

            return problemsFromDb;
        }

        public ICollection<Submission> GetAllProblemSubmissions(string problemId)
        {
            var submissions = this.context
                .Submissions
                .Include(s => s.Problem)
                .Include(s => s.User)
                .Where(s => s.ProblemId == problemId)
                .ToList();

            return submissions;
        }

        public int GetAllProblemSubscriptionsCount(string problemId)
        {
            var problemSubscriptionsCount = this.context
                .Submissions
                .Where(s => s.ProblemId == problemId)
                .ToList()
                .Count;

            return problemSubscriptionsCount;
        }

        public Problem GetProblemById(string problemId)
        {
            var problemFromDb = this.context
                .Problems
                .SingleOrDefault(p => p.Id == problemId);

            return problemFromDb;
        }
    }
}
