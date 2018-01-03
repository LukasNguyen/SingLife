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

        public static FacebookBonusCalculationInput GetListNotSortingByDateWithLargerBonusOfPoint()
        {
            //var sorter = A.Fake<IPolicySortService>();

            Policy policy2 = new Policy { PolicyNumber = "P002", Premium = 300, StartDate = new DateTime(2017, 11, 8) };
            Policy policy1 = new Policy { PolicyNumber = "P001", Premium = 200, StartDate = new DateTime(2016, 5, 12) };
            Policy[] policies = new Policy[] { policy2, policy1 };

            // Policy[] policies2 = A.Fake<Policy[]>();
            //A.CallTo(() => sorter.Sort(policies)).Returns(policies2);

            FacebookBonusSettings facebookBonusSettings = new FacebookBonusSettings { BonusPercentage = 0.03F, MaximumBonus = 10 };
            return new FacebookBonusCalculationInput { PoliciesOfCustomer = policies, Settings = facebookBonusSettings };
        }

        public static FacebookBonusCalculationInput GetListNotSortingByDateWithSmallerBonusOfPoint()
        {
            //var sorter = A.Fake<IPolicySortService>();

            Policy policy2 = new Policy { PolicyNumber = "P002", Premium = 100, StartDate = new DateTime(2017, 11, 8) };
            Policy policy1 = new Policy { PolicyNumber = "P001", Premium = 200, StartDate = new DateTime(2016, 5, 12) };
            Policy[] policies = new Policy[] { policy2, policy1 };

            // Policy[] policies2 = A.Fake<Policy[]>();
            //A.CallTo(() => sorter.Sort(policies)).Returns(policies2);

            FacebookBonusSettings facebookBonusSettings = new FacebookBonusSettings { BonusPercentage = 0.03F, MaximumBonus = 10 };
            return new FacebookBonusCalculationInput { PoliciesOfCustomer = policies, Settings = facebookBonusSettings };
        }

        //
        // TODO: Add constructor logic here
        //
        [Test]
        public void FacebookBonusCalculator_BonusInPointLargerThanMaximumBonus_TotalIsCorrect()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var output = facebookBonusCalculator.Calculate(GetListSortingByDate());

            const int expectedTotal = 10;

            Assert.AreEqual(expectedTotal, output.Total);
        }

        [Test]
        public void FacebookBonusCalculator_BonusInPointSmallerThanMaximumBonus_TotalIsCorrect()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var output = facebookBonusCalculator.Calculate(GetListSortingByDateWithSmallBonusOfPoint());

            const int expectedTotal = 9;


            Assert.AreEqual(expectedTotal, output.Total);
        }

        [Test]
        public void FacebookBonusCalculatorFacebookBonus_BonusInPointLargerThanMaximumBonus_ListPolicyBonusEqual()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var output = facebookBonusCalculator.Calculate(GetListSortingByDate());


            const string expected = "P001 P002 10";

            Assert.AreEqual(expected, output.ToString());
        }

        [Test]
        public void FacebookBonusCalculator_BonusInPointSmallerThanMaximumBonus_ListPolicyBonusEqual()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var output = facebookBonusCalculator.Calculate(GetListSortingByDateWithSmallBonusOfPoint());

            const string expected = "P001 P002 9";

            Assert.AreEqual(expected, output.ToString());
        }

        [Test]
        public void FacebookBonusCalculator_ListPolicyNotSortingByDateWithBonusLarger_TotalIsCorrect()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var output = facebookBonusCalculator.Calculate(GetListNotSortingByDateWithLargerBonusOfPoint());

            const int expectedTotal = 10;

            Assert.AreEqual(expectedTotal, output.Total);
        }
        [Test]
        public void FacebookBonusCalculator_ListPolicyNotSortingByDateWithBonusSmaller_TotalIsCorrect()
        {
            FacebookBonusCalculator facebookBonusCalculator = new FacebookBonusCalculator();
            var output = facebookBonusCalculator.Calculate(GetListNotSortingByDateWithSmallerBonusOfPoint());

            const int expectedTotal = 9;

            Assert.AreEqual(expectedTotal, output.Total);
        }
    }
}