using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravity.Utilities
{
    public static class MathUtilities
    {
        public static T Max<T>(params T[] ts)
        {
            return ts.Max();
        }
        public static T Min<T>(params T[] ts)
        {
            return ts.Min();
        }
    }
}
