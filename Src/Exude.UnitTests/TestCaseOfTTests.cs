using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Grean.Exude;

namespace Grean.Exude.UnitTests
{
    public class TestCaseOfTTests
    {
        [Fact]
        public void SutIsTestCase()
        {
            Action<object> dummyAction = _ => { };
            var sut = new TestCase<object>(dummyAction);
            Assert.IsAssignableFrom<ITestCase>(sut);
        }

        [Fact]
        public void TestActionIsCorrect()
        {
            Action<Version> expected = _ => { };
            var sut = new TestCase<Version>(expected);

            Action<Version> actual = sut.TestAction;

            Assert.Equal(expected, actual);
        }
    }
}
