namespace SULS.Services
{
    using SULS.Models;

    public interface ISubmissionService
    {
        bool CreateSubmission(Submission submission);

        bool DeleteSubmission(string submissionId);
    }
}
