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
            // TODO: Implement bonus calculation.

            var facebookBonusSetting = input.Settings;
            var sumBonusInPoints = 0;

            var lstPolicies = facebookBonusSetting.PolicySorter.Sort(input.PoliciesOfCustomer);

            var lstPolicyBonuses = new List<PolicyBonus>();

            var count = 0;

            foreach (Policy item in lstPolicies)
            {
                count++;
                var bonusInPoints = (float)item.Premium * facebookBonusSetting.BonusPercentage;
                var bonusInPointsAfterFloor = (int)Math.Floor(bonusInPoints);

                sumBonusInPoints += bonusInPointsAfterFloor;

                var policyBonus = new PolicyBonus
                {
                    PolicyNumber = item.PolicyNumber,
                    BonusInPoints = bonusInPointsAfterFloor
                };

                lstPolicyBonuses.Add(policyBonus);
                if (sumBonusInPoints > facebookBonusSetting.MaximumBonus)
                {
                    policyBonus.BonusInPoints -= (lstPolicyBonuses.Sum(n => n.BonusInPoints) - (int)facebookBonusSetting.MaximumBonus);

                    var remainingItems = lstPolicies.Skip(count);

                    if (remainingItems.Any())
                    {
                        foreach (Policy jtem in remainingItems)
                        {
                            var policyBonusZeroPoint = new PolicyBonus
                            {
                                PolicyNumber = jtem.PolicyNumber,
                                BonusInPoints = 0
                            };

                            lstPolicyBonuses.Add(policyBonusZeroPoint);
                        }
                    }

                    return new FacebookBonus
                    {
                        PolicyBonuses = lstPolicyBonuses.ToArray()
                    };
                }
            }
            return new FacebookBonus
            {
                PolicyBonuses = lstPolicyBonuses.ToArray()
            };
        }
    }
}