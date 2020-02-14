namespace SULS.Services
{
    using System.Collections.Generic;

    using SULS.Models;

    public interface IProblemService
    {
        void CreateProblem(string name, int points);

        ICollection<Problem> GetAllProblems();

        ICollection<Submission> GetAllProblemSubmissions(string problemId);

        int GetAllProblemSubscriptionsCount(string problemId);

        Problem GetProblemById(string problemId);
    }
}
