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

        public bool CreateProblem(Problem problem)
        {
            this.context.Problems.Add(problem);
            this.context.SaveChanges();

            return true;
        }

        public ICollection<Problem> GetAllProblems()
        {
            var problems = this.context.Problems.ToList();

            return problems;
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

        public int GetCountOfProblemSubmissions(string problemId)
        {
            var submissionsCount = this.context
                .Submissions
                .Where(s => s.ProblemId == problemId)
                .ToList()
                .Count;

            return submissionsCount;
        }

        public Problem GetProblemById(string problemId)
        {
            var problem = this.context
                .Problems
                .SingleOrDefault(p => p.Id == problemId);

            return problem;
        }
    }
}
