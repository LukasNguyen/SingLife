using FakeItEasy;
using NUnit.Framework;
using SingLife.FacebookShareBonus.Model;
using System;

namespace SingLife.FacebookShareBonus.Tests
{
    /// <summary>
    /// Summary description for FacebookBonus
    /// </summary>
    [TestFixture]
    public class FacebookBonusCalculator_UnitTests
    {
        public FacebookBonusCalculator_UnitTests()
        {
        }

        public static FacebookBonusCalculationInput GetListSortingByDateWithSmallBonusOfPoint()
        {
            Policy policy1 = new Policy
            {
                PolicyNumber = "P001",
                Premium = 200,
                StartDate = new DateTime(2016, 5, 12)
            };

            Policy policy2 = new Policy
            {
                PolicyNumber = "P002",
                Premium = 100,
                StartDate = new DateTime(2017, 11, 8)
            };

            Policy[] policies = new Policy[]
            {
                policy1,
                policy2
            };

            FacebookBonusSettings facebookBonusSettings = new FacebookBonusSettings
            {
                BonusPercentage = 0.03F,
                MaximumBonus = 10
            };

            return new FacebookBonusCalculationInput
            {
                PoliciesOfCustomer = policies,
                Settings = facebookBonusSettings
            };
        }

        public static FacebookBonusCalculationInput GetListSortingByDate()
        {
            Policy policy1 = new Policy
            {
                PolicyNumber = "P001",
                Premium = 200,
                StartDate = new DateTime(2016, 5, 12)
            };

            Policy policy2 = new Policy
            {
                PolicyNumber = "P002",
                Premium = 300,
                StartDate = new DateTime(2017, 11, 8)
            };

            Policy[] policies = new Policy[]
            {
                policy1,
                policy2
            };

            FacebookBonusSettings facebookBonusSettings = new FacebookBonusSettings
            {
                BonusPercentage = 0.03F,
                MaximumBonus = 10
            };

            return new FacebookBonusCalculationInput
            {
                PoliciesOfCustomer = policies,
                Settings = facebookBonusSettings
            };
        }

        public static FacebookBonusCalculationInput GetListNotSortingByDateWithLargerBonusOfPoint()
        {
            IPolicySortService sorter = A.Fake<IPolicySortService>();

            Policy policy2 = new Policy
            {
                PolicyNumber = "P002",
                Premium = 300,
                StartDate = new DateTime(2017, 11, 8)
            };

            Policy policy1 = new Policy
            {
                PolicyNumber = "P001",
                Premium = 200,
                StartDate = new DateTime(2016, 5, 12)
            };

            Policy[] policies = new Policy[]
            {
                policy2,
                policy1
            };

            A.CallTo(() => sorter.Sort(policies)).Returns(policies);

            FacebookBonusSettings facebookBonusSettings = new FacebookBonusSettings
            {
                BonusPercentage = 0.03F,
                MaximumBonus = 10,
                PolicySorter = sorter
             };

            return new FacebookBonusCalculationInput
            {
                PoliciesOfCustomer = policies,
                Settings = facebookBonusSettings
            };
        }

        public static FacebookBonusCalculationInput GetListNotSortingByDateWithSmallerBonusOfPoint()
        {
            IPolicySortService sorter = A.Fake<IPolicySortService>();

            Policy policy2 = new Policy
            {
                PolicyNumber = "P002",
                Premium = 100,
                StartDate = new DateTime(2017, 11, 8)
            };

            Policy policy1 = new Policy
            {
                PolicyNumber = "P001",
                Premium = 200,
                StartDate = new DateTime(2016, 5, 12)
            };

            Policy[] policies = new Policy[]
            {
                policy2,
                policy1
            };

            A.CallTo(() => sorter.Sort(policies)).Returns(policies);

            FacebookBonusSettings facebookBonusSettings = new FacebookBonusSettings
            {
                BonusPercentage = 0.03F,
                MaximumBonus = 10,
                PolicySorter = sorter
            };

            return new FacebookBonusCalculationInput
            {
                PoliciesOfCustomer = policies,
                Settings = facebookBonusSettings
            };
        }
        //
        // TODO: Add constructor logic here
        //
        [Test]
        public void FacebookBonusCalculator_BonusInPointLargerThanMaximumBonus_TotalIsCorrect()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(GetListSortingByDate());


            int actual = facebookBonus.Total;

            const int expectedTotal = 10;

            Assert.AreEqual(expectedTotal, actual);
        }

        [Test]
        public void FacebookBonusCalculator_BonusInPointSmallerThanMaximumBonus_TotalIsCorrect()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(GetListSortingByDateWithSmallBonusOfPoint());

            int actual = facebookBonus.Total;

            const int expectedTotal = 9;


            Assert.AreEqual(expectedTotal, actual);
        }

        [Test]
        public void FacebookBonusCalculatorFacebookBonus_BonusInPointLargerThanMaximumBonus_ListPolicyBonusEqual()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var actual = facebookBonusCalculator.Calculate(GetListSortingByDate());


            const string expected = "P001 P002 10";

            Assert.AreEqual(expected, actual.ToString());
        }

        [Test]
        public void FacebookBonusCalculator_BonusInPointSmallerThanMaximumBonus_ListPolicyBonusEqual()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var actual = facebookBonusCalculator.Calculate(GetListSortingByDateWithSmallBonusOfPoint());

            const string expected = "P001 P002 9";

            Assert.AreEqual(expected, actual.ToString());
        }

        [Test]
        public void FacebookBonusCalculator_ListPolicyNotSortingByDateWithBonusLarger_TotalIsCorrect()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(GetListNotSortingByDateWithLargerBonusOfPoint());

            int actual = facebookBonus.Total;

            const int expectedTotal = 10;


            Assert.AreEqual(expectedTotal, actual);
        }
        [Test]
        public void FacebookBonusCalculator_ListPolicyNotSortingByDateWithBonusSmaller_TotalIsCorrect()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(GetListNotSortingByDateWithSmallerBonusOfPoint());

            int actual = facebookBonus.Total;

            const int expectedTotal = 9;

            Assert.AreEqual(expectedTotal, actual);
        }
    }
}