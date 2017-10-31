using System.Diagnostics;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace noter.Common
{
    public static class Utils
    {
        [Conditional("DEBUG")]
        public static void Assert(bool expr)
        {
            System.Diagnostics.Debug.Assert(expr);
        }
    }
}