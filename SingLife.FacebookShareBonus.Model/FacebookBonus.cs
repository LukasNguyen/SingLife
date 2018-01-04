using System.Linq;

namespace SingLife.FacebookShareBonus.Model
{
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
}