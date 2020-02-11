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

        public bool CreateSubmission(Submission submission, string userId)
        {
            var userFromDb = this.context.Users.SingleOrDefault(u => u.Id == userId);
            submission.User = userFromDb;

            this.context.Submissions.Add(submission);
            this.context.SaveChanges();

            return true;
        }
    }
}
