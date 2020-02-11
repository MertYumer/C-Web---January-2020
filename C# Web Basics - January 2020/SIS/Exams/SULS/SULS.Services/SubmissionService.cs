namespace SULS.Services
{
    using System.Linq;

    using SULS.Data;
    using SULS.Models;

    public class SubmissionService : ISubmissionService
    {
        private readonly SulsDbContext context;

        public SubmissionService(SulsDbContext context)
        {
            this.context = context;
        }

        public bool CreateSubmission(Submission submission)
        {
            this.context.Submissions.Add(submission);
            this.context.SaveChanges();

            return true;
        }

        public bool DeleteSubmission(string submissionId)
        {
            var submission = this.context
                .Submissions
                .SingleOrDefault(s => s.Id == submissionId);

            if (submission == null)
            {
                return false;
            }

            this.context.Submissions.Remove(submission);
            this.context.SaveChanges();

            return true;
        }
    }
}
