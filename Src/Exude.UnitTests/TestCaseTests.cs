using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Grean.Exude;
using Xunit.Sdk;
using System.Reflection;

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

        [Fact]
        public void ConstructWithNullTestActionThrows()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TestCase(null));
        }

        [Fact]
        public void ConvertToTestCommandReturnsCorrectResult()
        {
            Action<object> expected = _ => { };
            var sut = new TestCase(expected);

            ITestCommand actual = sut.ConvertToTestCommand(dummyMethod);

            var fcc = Assert.IsAssignableFrom<FirstClassCommand>(actual);
            Assert.Equal(expected, fcc.TestAction);
        }

        private readonly static IMethodInfo dummyMethod = 
            Reflector.Wrap(typeof(TestCaseTests).GetMethod(
                "DummyTestMethod",
                BindingFlags.Static | BindingFlags.NonPublic));

        private void DummyTestMethod()
        {
        }

        [Fact]
        public void ConvertToTestCommandWithNullMethodThrows()
        {
            Action<object> dummyAction = _ => { };
            var sut = new TestCase(dummyAction);

            Assert.Throws<ArgumentNullException>(
                () => sut.ConvertToTestCommand(null));
        }
    }
}
