namespace SULS.Services
{
    public interface ISubmissionService
    {
        void CreateSubmission(string code, string problemId, string userId);

        void DeleteSubmission(string submissionId);
    }
}
