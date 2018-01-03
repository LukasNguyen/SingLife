using System;
using System.Collections.Generic;
using System.Linq;

namespace SingLife.FacebookShareBonus.Model
{
    ///<summary>
    /// /// A domain service that calculates the bonus rewarded to a customer who has shared to Facebook
    /// ///</summary>
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

            var lstPolicies = input.PoliciesOfCustomer.OrderBy(n => n.StartDate);

            List<PolicyBonus> lstPolicyBonuses = new List<PolicyBonus>();

            foreach (var item in lstPolicies)
            {
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
                    var temp = lstPolicyBonuses.ToList();
                    temp.RemoveAt(temp.Count - 1);
                    lstPolicyBonuses.Last().BonusInPoints = (int)facebookBonusSetting.MaximumBonus - temp.Sum(n => n.BonusInPoints);

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

    /// <summary>
    /// Contract of a policy-sorting service.
    /// </summary>
    public interface IPolicySortService
    {
        /// <summary>
        //// Sorts the specified policies.
        /// </summary>
        /// <param name="policies">An enumerable of policies to be sorted.</param>
        /// <returns>An ordered enumerable of policies.</returns>
        IEnumerable<Policy> Sort(IEnumerable<Policy> policies);
    }

    /// <summary>
    /// Represents the bonus rewarded to a customer for sharing to Facebook.
    /// </summary>
    public class FacebookBonus
    {
        public PolicyBonus[] PolicyBonuses { get; set; }

        public int Total
        {
            get
            {
                // TODO: Implement bonus total calculation.
                return PolicyBonuses.Sum(n => n.BonusInPoints);
            }
        }

        public override string ToString()
        {
            string s = "";
            foreach (var item in PolicyBonuses)
            {
                s += item.PolicyNumber + " ";
            }
            return s + Total.ToString();
        }
    }

    public class PolicyBonus
    {
        public string PolicyNumber { get; set; }
        public int BonusInPoints { get; set; }
    }

    //public class MyPolicySort : IPolicySortService
    //{
    //    public IEnumerable<Policy> Sort(IEnumerable<Policy> policies)
    //    {
    //        return policies.OrderBy(n => n.StartDate);
    //    }
    //}
}