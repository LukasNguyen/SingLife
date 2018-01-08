using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SingLife.FacebookShareBonus.Model.Tests.UT
{
    public class FacebookBonusCalculatorTests2
    {
        [TestFixture]
        public class CalculateTests
        {
            [Test]
            public void Total_bonus_should_be_correct_when_total_bonus_is_less_than_maximum_bonus()
            {
                // Arrange
                var facebookBonusCalculator = CreateCalculator();
                var input = SampleInput(35);
                const int expectedTotal = 30;

                // Act
                var facebookBonus = facebookBonusCalculator.Calculate(input);

                // Assert
                var actualTotal = facebookBonus.Total;
                Assert.That(actualTotal, Is.EqualTo(expectedTotal));
            }

            [Test]
            public void Policy_bonuses_should_be_correct_when_total_bonus_is_less_than_maximum_bonus()
            {
                // Arrange
                var facebookBonusCalculator = CreateCalculator();
                var input = SampleInput(35);

                var expected = new[]
                {
                    new PolicyBonus { PolicyNumber = "P001", BonusInPoints = 6 },
                    new PolicyBonus { PolicyNumber = "P002", BonusInPoints = 9 },
                    new PolicyBonus { PolicyNumber = "P003", BonusInPoints = 6 },
                    new PolicyBonus { PolicyNumber = "P004", BonusInPoints = 9 }
                };

                // Act
                var facebookBonus = facebookBonusCalculator.Calculate(input);

                // Assert
                var actual = facebookBonus.PolicyBonuses;
                Assert.That(actual, Is.EqualTo(expected), "Policies should be rewarded with correct points.");
            }

            [Test]
            public void Total_bonus_should_be_correct_when_total_bonus_is_greater_than_maximum_bonus()
            {
                // Arrange
                var facebookBonusCalculator = CreateCalculator();
                const int expectedTotal = 25;
                var input = SampleInput(25);

                // Act
                var facebookBonus = facebookBonusCalculator.Calculate(input);

                // Assert
                var actualTotal = facebookBonus.Total;
                Assert.AreEqual(expectedTotal, actualTotal);
            }

            [Test]
            public void Last_rewarded_policy_should_have_correct_bonus_when_total_bonus_is_greater_than_maximum_bonus()
            {
                // Arrange
                var facebookBonusCalculator = CreateCalculator();
                var expected = new PolicyBonus
                {
                    PolicyNumber = "P002",
                    BonusInPoints = 6
                };
                var input = SampleInput(12);

                // Act
                var facebookBonus = facebookBonusCalculator.Calculate(input);

                // Assert
                var actual = facebookBonus.PolicyBonuses[1];
                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void Exceeded_policies_should_have_zero_points_when_total_bonus_is_greater_than_maximum_bonus()
            {
                // Arrange
                var facebookBonusCalculator = CreateCalculator();

                var expectedExceededPolicies = new[]
                {
                    new PolicyBonus
                    {
                        PolicyNumber = "P003",
                        BonusInPoints = 0
                    },
                    new PolicyBonus
                    {
                        PolicyNumber = "P004",
                        BonusInPoints = 0
                    }
                };

                var input = SampleInput(14);

                // Act
                var facebookBonus = facebookBonusCalculator.Calculate(input);

                // Assert
                var actualPolicies = facebookBonus.PolicyBonuses;
                Assert.That(actualPolicies, Is.SupersetOf(expectedExceededPolicies), "Exceeded policies should not be rewarded.");
            }

            [Test]
            public void Calculate_function_should_call_policy_sort_service()
            {
                // Arrange
                var facebookBonusCalculator = CreateCalculator();

                var input = SampleInputWithFakeSortService();
                var mockSortService = input.Settings.PolicySorter;

                // Act
                facebookBonusCalculator.Calculate(input);

                // Assert
                A.CallTo(() => mockSortService.Sort(input.PoliciesOfCustomer)).MustHaveHappened();
            }
        }

        private static FacebookBonusCalculator2 CreateCalculator()
        {
            return new FacebookBonusCalculator2();
        }

        private static FacebookBonusCalculationInput SampleInput(int maximumBonus)
        {
            var policies = new[]
            {
                new Policy
                {
                    PolicyNumber = "P001",
                    Premium = 200,
                    StartDate = new DateTime(2016, 5, 12)
                },
                new Policy
                {
                    PolicyNumber = "P002",
                    Premium = 300,
                    StartDate = new DateTime(2017, 11, 8)
                },
                new Policy
                {
                    PolicyNumber = "P003",
                    Premium = 200,
                    StartDate = new DateTime(2017, 12, 12)
                },
                new Policy
                {
                    PolicyNumber = "P004",
                    Premium = 300,
                    StartDate = new DateTime(2017, 12, 15)
                }
            };

            var sorter = CreateFakeSortService(policies);

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

        private static FacebookBonusCalculationInput SampleInputWithFakeSortService()
        {
            var input = SampleInput(10);
            input.Settings.PolicySorter = CreateFakeSortService(input.PoliciesOfCustomer);

            return input;
        }

        private static IPolicySortService CreateFakeSortService(IEnumerable<Policy> policies)
        {
            var mockSortService = A.Fake<IPolicySortService>();

            A.CallTo(() => mockSortService.Sort(policies))
                .Returns(policies);

            return mockSortService;
        }
    }
}