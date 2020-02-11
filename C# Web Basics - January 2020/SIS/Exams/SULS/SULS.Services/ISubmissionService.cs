namespace SULS.Services
{
    using SULS.Models;

    public interface ISubmissionService
    {
        bool CreateSubmission(Submission submission, string userId);
    }
}
