using System;
using System.Collections.Generic;
using System.Linq;

namespace SingLife.FacebookShareBonus.Model
{


    /// <summary>
    /// Contract of a policy-sorting service.
    /// </summary>
    public interface IPolicySortService
    {
        /// <summary>
        /// Sorts the specified policies.
        /// </summary>
        /// <param name="policies">An enumerable of policies to be sorted.</param>
        /// <returns>An ordered enumerable of policies.</returns>
        IEnumerable<Policy> Sort(IEnumerable<Policy> policies);
    }
}