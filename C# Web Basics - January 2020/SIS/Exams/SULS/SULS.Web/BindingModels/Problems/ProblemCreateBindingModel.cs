namespace SULS.Web.BindingModels.Problems
{
    using SIS.MvcFramework.Attributes.Validation;

    public class ProblemCreateBindingModel
    {
        private const string NameErrorMessage = "Name must be between 5 and 20 symbols long.";

        private const string PointsErrorMessage = "Points must be between than 50 and 300.";

        [RequiredSis]
        [StringLengthSis(5, 20, NameErrorMessage)]
        public string Name { get; set; }


        [RequiredSis]
        [RangeSis(50, 300, PointsErrorMessage)]
        public int Points { get; set; }
    }
}
