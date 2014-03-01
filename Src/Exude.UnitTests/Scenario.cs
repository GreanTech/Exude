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
        public static IEnumerable<ITestCase> YieldFirstClassTests()
        {
            yield return new TestCase(_ => Assert.Equal(1, 1));
            yield return new TestCase(_ => Assert.Equal(2, 2));
            yield return new TestCase(_ => Assert.Equal(3, 3));
        }

        [FirstClassTests]
        public static ITestCase[] ProjectTestCasesAsArray()
        {
            var testCases = new[] {
                new { x =  1, y =    2 },
                new { x =  3, y =    8 },
                new { x = 42, y = 1337 },
            };
            return testCases
                .Select(tc => new TestCase(_ =>
                    Assert.True(tc.x < tc.y)))
                .ToArray();
        }
    }
}
