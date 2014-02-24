using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Grean.Exude.UnitTests
{
    public class Scenario
    {
        [FirstClassTests]
        public static IEnumerable<FirstClassCommand> YieldFirstClassTests()
        {
            yield return new FirstClassCommand(_ => Assert.Equal(1, 1));
            yield return new FirstClassCommand(_ => Assert.Equal(2, 2));
            yield return new FirstClassCommand(_ => Assert.Equal(3, 3));
        }
    }
}
