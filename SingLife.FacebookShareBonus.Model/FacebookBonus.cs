using System.Collections.Generic;
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
    }

    public class PolicyBonus
    {
        public string PolicyNumber { get; set; }

        public int BonusInPoints { get; set; }

        public override bool Equals(object obj)
        {
            var bonus = obj as PolicyBonus;
            return bonus != null &&
                   PolicyNumber == bonus.PolicyNumber &&
                   BonusInPoints == bonus.BonusInPoints;
        }

        public override int GetHashCode()
        {
            var hashCode = -732577826;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PolicyNumber);
            hashCode = hashCode * -1521134295 + BonusInPoints.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(PolicyBonus bonus1, PolicyBonus bonus2)
        {
            return EqualityComparer<PolicyBonus>.Default.Equals(bonus1, bonus2);
        }

        public static bool operator !=(PolicyBonus bonus1, PolicyBonus bonus2)
        {
            return !(bonus1 == bonus2);
        }
    }
}