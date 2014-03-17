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

            yield return new TestCase(() => Assert.Equal(1, 1));
            yield return new TestCase(() => Assert.Equal(2, 2));
            yield return new TestCase(() => Assert.Equal(3, 3));
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

        public void AParameterizedTest(DateTimeOffset x, DateTimeOffset y)
        {
            Assert.True(x < y);
        }

        [FirstClassTests]
        public static TestCase<Scenario>[] RunAParameterizedTest()
        {
            var testCases = new[] 
            {
                new 
                {
                    x = new DateTimeOffset(2002, 10, 12, 18, 15, 0, TimeSpan.FromHours(1)),
                    y = new DateTimeOffset(2007,  4, 21, 18, 15, 0, TimeSpan.FromHours(1))
                },
                new
                {
                    x = new DateTimeOffset(1970, 11, 25, 16, 10, 0, TimeSpan.FromHours(1)),
                    y = new DateTimeOffset(1972,  6,  6,  8,  5, 0, TimeSpan.FromHours(1))
                },
                new
                {
                    x = new DateTimeOffset(2014, 3, 2, 17, 18, 45, TimeSpan.FromHours(1)),
                    y = new DateTimeOffset(2014, 3, 2, 17, 18, 45, TimeSpan.FromHours(0))
                }
            };
            return testCases
                .Select(tc =>
                    new TestCase<Scenario>(
                        s => s.AParameterizedTest(tc.x, tc.y)))
                .ToArray();
        }
    }
}
