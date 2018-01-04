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
            int sumBonusInPoints = 0;

            var lstPolicies = facebookBonusSetting.PolicySorter.Sort(input.PoliciesOfCustomer);

            List<PolicyBonus> lstPolicyBonuses = new List<PolicyBonus>();

            int count = 0;

            foreach (Policy item in lstPolicies)
            {
                count++;
                float bonusInPoints = (float)item.Premium * facebookBonusSetting.BonusPercentage;
                int bonusInPointsAfterFloor = (int)Math.Floor(bonusInPoints);

                sumBonusInPoints += bonusInPointsAfterFloor;

                PolicyBonus policyBonus = new PolicyBonus
                {
                    PolicyNumber = item.PolicyNumber,
                    BonusInPoints = bonusInPointsAfterFloor
                };

                lstPolicyBonuses.Add(policyBonus);
                if (sumBonusInPoints > facebookBonusSetting.MaximumBonus)
                {
                    policyBonus.BonusInPoints -= (lstPolicyBonuses.Sum(n => n.BonusInPoints) - (int)facebookBonusSetting.MaximumBonus);

                    var remainingItem = lstPolicies.Skip(count);

                    if (remainingItem.Any())
                    {
                        foreach (Policy jtem in remainingItem)
                        {
                            PolicyBonus policyBonusZero = new PolicyBonus
                            {
                                PolicyNumber = jtem.PolicyNumber,
                                BonusInPoints = 0
                            };
                            lstPolicyBonuses.Add(policyBonusZero);
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