using System;

namespace SingLife.FacebookShareBonus.Model
{
    /// <summary>
    /// Parameter class of the <see cref="FacebookBonusCalculator.Calculate(FacebookBonusCalculationInput)"/> method.
    /// </summary>
    public class FacebookBonusCalculationInput
    {
        /// <summary>
        /// A collection of policies owned by the customer.
        /// </summary>
        public Policy[] PoliciesOfCustomer { get; set; }

        /// <summary>
        /// Facebook bonus calculation settings.
        /// </summary>
        public FacebookBonusSettings Settings { get; set; }
    }

    public class Policy
    {
        public string PolicyNumber { get; set; }
        public decimal Premium { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class FacebookBonusSettings
    {
        public float BonusPercentage { get; set; }
        public decimal MaximumBonus { get; set; }
        public IPolicySortService PolicySorter { get; set; }
    }
}