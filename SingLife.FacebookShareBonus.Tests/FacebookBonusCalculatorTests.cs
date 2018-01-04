using FakeItEasy;
using NUnit.Framework;
using System;

namespace SingLife.FacebookShareBonus.Model.UnitTests
{
    [TestFixture]
    public class FacebookBonusCalculatorTests
    {
        [Test]
        public void FacebookBonusCalculatorFacebookBonus_BonusInPointLargerThanMaximumBonusAndRemainingItemZeroPoint_ListPolicyBonusEqual()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var actual = facebookBonusCalculator.Calculate(GetListWithLargerBonusOfPointAndRemainingItemsZeroPoint());

            const string expected = "P001 P002 P003 10";

            Assert.AreEqual(expected, actual.ToString());
        }

        [Test]
        public void FacebookBonusCalculatorFacebookBonus_BonusInPointLargerThanMaximumBonus_ListPolicyBonusEqual()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var actual = facebookBonusCalculator.Calculate(GetListWithLargerBonusOfPoint());

            const string expected = "P001 P002 10";

            Assert.AreEqual(expected, actual.ToString());
        }

        [Test]
        public void FacebookBonusCalculator_BonusInPointSmallerThanMaximumBonus_ListPolicyBonusEqual()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var actual = facebookBonusCalculator.Calculate(GetListWithSmallerBonusOfPoint());

            const string expected = "P001 P002 9";

            Assert.AreEqual(expected, actual.ToString());
        }

        [Test]
        public void FacebookBonusCalculator_BonusInPointLargerThanMaximumBonus_TotalIsCorrect()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(GetListWithLargerBonusOfPoint());

            int actual = facebookBonus.Total;

            const int expectedTotal = 10;

            Assert.AreEqual(expectedTotal, actual);
        }

        [Test]
        public void FacebookBonusCalculator_BonusInPointLargerThanMaximumBonusAndRemainingItemZeroPoint_TotalIsCorrect()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(GetListWithLargerBonusOfPointAndRemainingItemsZeroPoint());

            int actual = facebookBonus.Total;

            const int expectedTotal = 10;

            Assert.AreEqual(expectedTotal, actual);
        }

        [Test]
        public void FacebookBonusCalculator_BonusInPointSmallerThanMaximumBonus_TotalIsCorrect()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(GetListWithSmallerBonusOfPoint());

            int actual = facebookBonus.Total;

            const int expectedTotal = 9;

            Assert.AreEqual(expectedTotal, actual);
        }

        private static FacebookBonusCalculationInput GetListWithLargerBonusOfPoint()
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
                policy1,
                policy2,
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

        private static FacebookBonusCalculationInput GetListWithLargerBonusOfPointAndRemainingItemsZeroPoint()
        {
            IPolicySortService sorter = A.Fake<IPolicySortService>();

            Policy policy1 = new Policy
            {
                PolicyNumber = "P001",
                Premium = 300,
                StartDate = new DateTime(2017, 11, 8)
            };

            Policy policy2 = new Policy
            {
                PolicyNumber = "P002",
                Premium = 200,
                StartDate = new DateTime(2016, 5, 12)
            };

            Policy policy3 = new Policy
            {
                PolicyNumber = "P003",
                Premium = 200,
                StartDate = new DateTime(2016, 12, 9)
            };

            Policy[] policies = new Policy[]
            {
                policy1,
                policy2,
                policy3
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

        private static FacebookBonusCalculationInput GetListWithSmallerBonusOfPoint()
        {
            IPolicySortService sorter = A.Fake<IPolicySortService>();

            Policy policy2 = new Policy
            {
                PolicyNumber = "P001",
                Premium = 100,
                StartDate = new DateTime(2017, 11, 8)
            };

            Policy policy1 = new Policy
            {
                PolicyNumber = "P002",
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
    }
}