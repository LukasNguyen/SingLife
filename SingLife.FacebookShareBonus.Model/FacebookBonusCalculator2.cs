using System;
using System.Collections.Generic;
using System.Linq;

namespace SingLife.FacebookShareBonus.Model
{
    ///<summary>
    /// A domain service that calculates the bonus rewarded to a customer who has shared to Facebook.
    ///</summary>
    public class FacebookBonusCalculator2
    {
        /// <summary>
        ///  Calculates the bonus.
        /// </summary>
        /// <param name="input">A parameter object representing inputs to the calculation.</param>
        /// <returns>A <see cref="FacebookBonus"/> object.</returns>
        public virtual FacebookBonus Calculate(FacebookBonusCalculationInput input)
        {
            var sortedPolicies = input.Settings.PolicySorter
                .Sort(input.PoliciesOfCustomer)
                .ToArray();

            var policyBonuses = new List<PolicyBonus>();

            foreach (Policy policy in sortedPolicies)
            {
                // Projection
                var policyBonus = new PolicyBonus
                {
                    PolicyNumber = policy.PolicyNumber,
                    BonusInPoints = CalculatePolicyBonus(policy, policyBonuses, input.Settings)
                };

                policyBonuses.Add(policyBonus);
            }

            return new FacebookBonus
            {
                PolicyBonuses = policyBonuses.ToArray()
            };
        }

        private int CalculatePolicyBonus(Policy currentPolicy, IEnumerable<PolicyBonus> policyBonuses, FacebookBonusSettings settings)
        {
            var currentTotalBonus = policyBonuses
                .Sum(bonus => bonus.BonusInPoints);

            var remainPoints = CalculateRemainingBonus(settings, currentTotalBonus);

            return CalculatePolicyBonus(currentPolicy, settings, remainPoints);
        }

        private static int CalculateRemainingBonus(FacebookBonusSettings settings, int currentTotalBonus)
        {
            var maximumPolicyBonus = (int)settings.MaximumBonus - currentTotalBonus;

            if (maximumPolicyBonus < 0)
            {
                maximumPolicyBonus = 0;
            }

            return maximumPolicyBonus;
        }

        private static int CalculatePolicyBonus(
            Policy currentPolicy,
            FacebookBonusSettings settings,
            int maximumPolicyBonus)
        {
            var policyBonus = (float)currentPolicy.Premium * settings.BonusPercentage;

            var policyBonusAfterFloor = (int)Math.Floor(policyBonus);

            return policyBonusAfterFloor < maximumPolicyBonus ? policyBonusAfterFloor : maximumPolicyBonus;
        }
    }
}