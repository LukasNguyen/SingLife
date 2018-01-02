using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SingLife.FacebookShareBonus.Model;

namespace SingLife.FacebookShareBonus.Tests
{
    /// <summary>
    /// Summary description for FacebookBonus
    /// </summary>
    [TestFixture]
    public class FacebookShareBonus
    {
        public FacebookShareBonus()
        {
        }
        public class Initialize
        {
            public static FacebookBonusCalculationInput GetListSortingByDateWithSmallBonusOfPoint()
            {
                Policy policy1 = new Policy { PolicyNumber = "P001", Premium = 200, StartDate = new DateTime(2016, 5, 12) };
                Policy policy2 = new Policy { PolicyNumber = "P002", Premium = 100, StartDate = new DateTime(2017, 11, 8) };
                Policy[] policies = new Policy[] { policy1, policy2 };
                FacebookBonusSettings facebookBonusSettings = new FacebookBonusSettings { BonusPercentage = 0.03F, MaximumBonus = 10 };
                return new FacebookBonusCalculationInput { PoliciesOfCustomer = policies, Settings = facebookBonusSettings };
            }
            public static FacebookBonusCalculationInput GetListSortingByDate()
            {
                Policy policy1 = new Policy { PolicyNumber = "P001", Premium = 200, StartDate = new DateTime(2016, 5, 12) };
                Policy policy2 = new Policy { PolicyNumber = "P002", Premium = 300, StartDate = new DateTime(2017, 11, 8) };
                Policy[] policies = new Policy[] { policy1, policy2 };
                FacebookBonusSettings facebookBonusSettings = new FacebookBonusSettings { BonusPercentage = 0.03F, MaximumBonus = 10 };
                return new FacebookBonusCalculationInput { PoliciesOfCustomer = policies, Settings = facebookBonusSettings };
            }
            public static FacebookBonusCalculationInput GetListNotSortingByDate()
            {
                Policy policy2 = new Policy { PolicyNumber = "P002", Premium = 300, StartDate = new DateTime(2017, 11, 8) };
                Policy policy1 = new Policy { PolicyNumber = "P001", Premium = 200, StartDate = new DateTime(2016, 5, 12) };
                Policy[] policies = new Policy[] { policy1, policy2 };
                FacebookBonusSettings facebookBonusSettings = new FacebookBonusSettings { BonusPercentage = 0.03F, MaximumBonus = 10 };
                return new FacebookBonusCalculationInput { PoliciesOfCustomer = policies, Settings = facebookBonusSettings };
            }
        }
        //
        // TODO: Add constructor logic here
        //
        [Test]
        public void FacebookBonus_BonusInPointLargerThanMaximumBonus_TotalIsCorrect()
        {

            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var output = facebookBonusCalculator.Calculate(Initialize.GetListSortingByDate());

            PolicyBonus policyBonus1 = new PolicyBonus { PolicyNumber = "P001", BonusInPoints = 6 };
            PolicyBonus policyBonus2 = new PolicyBonus { PolicyNumber = "P002", BonusInPoints = 4 };
            PolicyBonus[] policyBonuses = new PolicyBonus[] { policyBonus1, policyBonus2 };
            FacebookBonus expected = new FacebookBonus { PolicyBonuses = policyBonuses };

            NUnit.Framework.Assert.AreEqual(expected.Total, output.Total);
        }
        [Test]
        public void FacebookBonus_BonusInPointSmallerThanMaximumBonus_TotalIsCorrect()
        {

            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var output = facebookBonusCalculator.Calculate(Initialize.GetListSortingByDateWithSmallBonusOfPoint());

            PolicyBonus policyBonus1 = new PolicyBonus { PolicyNumber = "P001", BonusInPoints = 6 };
            PolicyBonus policyBonus2 = new PolicyBonus { PolicyNumber = "P002", BonusInPoints = 3 };
            PolicyBonus[] policyBonuses = new PolicyBonus[] { policyBonus1, policyBonus2 };
            FacebookBonus expected = new FacebookBonus { PolicyBonuses = policyBonuses };

            NUnit.Framework.Assert.AreEqual(expected.Total, output.Total);
        }
        [Test]
        public void FacebookBonus_BonusInPointLargerThanMaximumBonus_ListPolicyBonusEqual()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var output = facebookBonusCalculator.Calculate(Initialize.GetListSortingByDate());

            PolicyBonus policyBonus1 = new PolicyBonus { PolicyNumber = "P001", BonusInPoints = 6 };
            PolicyBonus policyBonus2 = new PolicyBonus { PolicyNumber = "P002", BonusInPoints = 4 };
            PolicyBonus[] policyBonuses = new PolicyBonus[] { policyBonus1, policyBonus2 };
            FacebookBonus expected = new FacebookBonus { PolicyBonuses = policyBonuses };

            NUnit.Framework.Assert.AreEqual(expected.ToString(), output.ToString());
        }
        [Test]
        public void FacebookBonus_BonusInPointSmallerThanMaximumBonus_ListPolicyBonusEqual()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var output = facebookBonusCalculator.Calculate(Initialize.GetListSortingByDateWithSmallBonusOfPoint());

            PolicyBonus policyBonus1 = new PolicyBonus { PolicyNumber = "P001", BonusInPoints = 6 };
            PolicyBonus policyBonus2 = new PolicyBonus { PolicyNumber = "P002", BonusInPoints = 3 };
            PolicyBonus[] policyBonuses = new PolicyBonus[] { policyBonus1, policyBonus2 };
            FacebookBonus expected = new FacebookBonus { PolicyBonuses = policyBonuses };

            NUnit.Framework.Assert.AreEqual(expected.ToString(), output.ToString());
        }
        [Test]
        public void FacebookBonus_ListPolicyNotSortingByDate_ListPolicyBonusEqual()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var output = facebookBonusCalculator.Calculate(Initialize.GetListNotSortingByDate());

            PolicyBonus policyBonus1 = new PolicyBonus { PolicyNumber = "P001", BonusInPoints = 6 };
            PolicyBonus policyBonus2 = new PolicyBonus { PolicyNumber = "P002", BonusInPoints = 4 };
            PolicyBonus[] policyBonuses = new PolicyBonus[] { policyBonus1, policyBonus2 };
            FacebookBonus expected = new FacebookBonus { PolicyBonuses = policyBonuses };

            NUnit.Framework.Assert.AreEqual(expected.ToString(), output.ToString());
        }
    }
}
