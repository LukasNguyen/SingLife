using System;
using System.Collections.Generic;
using System.Linq;

namespace SingLife.FacebookShareBonus.Model
{
    ///<summary>
    /// A domain service that calculates the bonus rewarded to a customer who has shared to Facebook.
    ///</summary>
    public class FacebookBonusCalculator
    {
        /// <summary>
        ///  Calculates the bonus.
        /// </summary>
        /// <param name="input">A parameter object representing inputs to the calculation.</param>
        /// <returns>A <see cref="FacebookBonus"/> object.</returns>
        public virtual FacebookBonus Calculate(FacebookBonusCalculationInput input)
        {
            var facebookBonusSetting = input.Settings;

            var sortedPolicies = facebookBonusSetting.PolicySorter.Sort(input.PoliciesOfCustomer);

            var policyBonuses = new List<PolicyBonus>();

            var count = 0;
            var sumBonusInPoints = 0;

            foreach (Policy policy in sortedPolicies)
            {
                var bonusInPoints = CalculateBonusInPoints(facebookBonusSetting, policy);

                var policyBonus = AddNewPolicy(policyBonuses, policy, bonusInPoints);

                count++;
                sumBonusInPoints += bonusInPoints;

                if (sumBonusInPoints > facebookBonusSetting.MaximumBonus)
                {
                    CalculateBonusInPointsWhenSumBonusInPointsLargerThanMaximumBonus(facebookBonusSetting, policyBonuses, policyBonus);

                    var remainingPolicies = sortedPolicies.Skip(count);

                    AssignZeroPointsForExceedPolicy(policyBonuses, remainingPolicies);

                    return new FacebookBonus
                    {
                        PolicyBonuses = policyBonuses.ToArray()
                    };
                }
            }
            return new FacebookBonus
            {
                PolicyBonuses = policyBonuses.ToArray()
            };
        }

        private static void CalculateBonusInPointsWhenSumBonusInPointsLargerThanMaximumBonus(FacebookBonusSettings facebookBonusSetting, List<PolicyBonus> policyBonuses, PolicyBonus policyBonus)
        {
            policyBonus.BonusInPoints -= (policyBonuses.Sum(n => n.BonusInPoints) - (int)facebookBonusSetting.MaximumBonus);
        }

        private static PolicyBonus AddNewPolicy(List<PolicyBonus> policyBonuses, Policy policy, int bonusInPointsAfterFloor)
        {
            var policyBonus = new PolicyBonus
            {
                PolicyNumber = policy.PolicyNumber,
                BonusInPoints = bonusInPointsAfterFloor
            };

            policyBonuses.Add(policyBonus);
            return policyBonus;
        }

        private static int CalculateBonusInPoints(FacebookBonusSettings facebookBonusSetting, Policy policy)
        {
            var bonusInPoints = (float)policy.Premium * facebookBonusSetting.BonusPercentage;
            return (int)Math.Floor(bonusInPoints);
        }

        private static void AssignZeroPointsForExceedPolicy(List<PolicyBonus> policyBonuses, IEnumerable<Policy> remainingPolicies)
        {
            if (remainingPolicies.Any())
            {
                foreach (Policy remainingPolicy in remainingPolicies)
                {
                    var exceedPolicy = new PolicyBonus
                    {
                        PolicyNumber = remainingPolicy.PolicyNumber,
                        BonusInPoints = 0
                    };

                    policyBonuses.Add(exceedPolicy);
                }
            }
        }
    }
}