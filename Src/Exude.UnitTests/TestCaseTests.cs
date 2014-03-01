using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Grean.Exude;

namespace Grean.Exude.UnitTests
{
    public class TestCaseTests
    {
        [Fact]
        public void SutIsTestCase()
        {
            Action<object> dummyAction = _ => { };
            var sut = new TestCase(dummyAction);
            Assert.IsAssignableFrom<ITestCase>(sut);
        }

        [Fact]
        public void TestActionIsCorrect()
        {
            Action<object> expected = _ => { };
            var sut = new TestCase(expected);

            Action<object> actual = sut.TestAction;

            Assert.Equal(expected, actual);
        }
    }
}
