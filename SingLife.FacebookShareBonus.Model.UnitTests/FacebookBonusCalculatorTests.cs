using FakeItEasy;
using NUnit.Framework;
using System;
using System.Linq;

namespace SingLife.FacebookShareBonus.Model.UnitTests
{
    [TestFixture]
    public class FacebookBonusCalculatorTests
    {
        [Test]
        public void FacebookBonusCalculator_BonusInPointSmallerThanMaximumBonus_TotalIsCorrect()
        {
            var facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(SampleInput(35));

            int actual = facebookBonus.Total;

            const int expectedTotal = 30;

            Assert.AreEqual(expectedTotal, actual);
        }

        [Test]
        public void FacebookBonusCalculator_TotalBonusLessThanMaximumBonus_PolicyBonusPointsAreCorrect()
        {
            var facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(SampleInput(35));

            var actual = facebookBonus.PolicyBonuses.ToArray();

            var policyBonus = new PolicyBonus
            {
                PolicyNumber = "P001",
                BonusInPoints = 6
            };

            var policyBonus2 = new PolicyBonus
            {
                PolicyNumber = "P002",
                BonusInPoints = 9
            };
            var policyBonus3 = new PolicyBonus
            {
                PolicyNumber = "P003",
                BonusInPoints = 6
            };

            var policyBonus4 = new PolicyBonus
            {
                PolicyNumber = "P004",
                BonusInPoints = 9
            };

            PolicyBonus[] expected =
            {
                policyBonus,
                policyBonus2,
                policyBonus3,
                policyBonus4
            };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void FacebookBonusCalculator_TotalBonusGreaterThanMaximumBonus_TotalIsCorrect()
        {
            var facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(SampleInput(25));

            int actual = facebookBonus.Total;

            const int expectedTotal = 25;

            Assert.AreEqual(expectedTotal, actual);
        }

        [Test]
        public void FacebookBonusCalculator_TotalBonusGreaterThanMaximumBonus_LastPolicyBonusPointAreCorrect()
        {
            var facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(SampleInput(25));

            var actual = facebookBonus.PolicyBonuses.Last();

            var expected = new PolicyBonus
            {
                PolicyNumber = "P004",
                BonusInPoints = 4
            };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void FacebookBonusCalculator_TotalBonusGreaterThanMaximumBonus_ExceededPoliciesWillHaveZeroPoints()
        {
            var facebookBonusCalculator = new FacebookBonusCalculator();
            FacebookBonus facebookBonus = facebookBonusCalculator.Calculate(SampleInput(14));

            var actual = facebookBonus.PolicyBonuses;

            var policyBonus = new PolicyBonus
            {
                PolicyNumber = "P003",
                BonusInPoints = 0
            };

            var policyBonus2 = new PolicyBonus
            {
                PolicyNumber = "P004",
                BonusInPoints = 0
            };

            PolicyBonus[] expectedExceededPolicies =
            {
                policyBonus,
                policyBonus2
            };

            Assert.That(facebookBonus.PolicyBonuses, Is.SupersetOf(expectedExceededPolicies));
        }

        private static FacebookBonusCalculationInput SampleInput(int maximumBonus)
        {
            var policy1 = new Policy
            {
                PolicyNumber = "P001",
                Premium = 200,
                StartDate = new DateTime(2016, 5, 12)
            };

            var policy2 = new Policy
            {
                PolicyNumber = "P002",
                Premium = 300,
                StartDate = new DateTime(2017, 11, 8)
            };

            var policy3 = new Policy
            {
                PolicyNumber = "P003",
                Premium = 200,
                StartDate = new DateTime(2017, 12, 12)
            };

            var policy4 = new Policy
            {
                PolicyNumber = "P004",
                Premium = 300,
                StartDate = new DateTime(2017, 12, 15)
            };

            Policy[] policies = {
                policy1,
                policy2,
                policy3,
                policy4
            };

            IPolicySortService sorter = A.Fake<IPolicySortService>();
            A.CallTo(() => sorter.Sort(policies)).Returns(policies);

            var facebookBonusSettings = new FacebookBonusSettings
            {
                BonusPercentage = 0.03F,
                MaximumBonus = maximumBonus,
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