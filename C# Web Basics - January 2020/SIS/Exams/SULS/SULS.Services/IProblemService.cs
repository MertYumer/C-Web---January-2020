namespace SULS.Services
{
    using System.Collections.Generic;

    using SULS.Models;

    public interface IProblemService
    {
        bool CreateProblem(Problem problem);

        ICollection<Problem> GetAllProblems();

        int GetCountOfProblemSubmissions(string problemId);

        Problem GetProblemById(string problemId);

        ICollection<Submission> GetAllProblemSubmissions(string problemId);
    }
}
