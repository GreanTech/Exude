using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Grean.Exude;
using System.Reflection;
using Xunit.Sdk;

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

        [Fact]
        public void ConstructWithNullTestActionThrows()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TestCase<Guid>(null));
        }

        [Fact]
        public void ConvertToTestCommandReturnsCorrectResult()
        {
            var verified = false;
            var input = new Version(1, 1);
            Action<Version> expected = v => verified = input.Equals(v);
            var sut = new TestCase<Version>(expected);

            ITestCommand actual = sut.ConvertToTestCommand(dummyMethod);

            var fcc = Assert.IsAssignableFrom<FirstClassCommand>(actual);
            fcc.TestAction(input);
            Assert.True(
                verified,
                "Invoking TestAction on the resulting FirstClassCommand should indirectly indicate that the original test command was invoked.");
        }

        private readonly static IMethodInfo dummyMethod =
            Reflector.Wrap(typeof(TestCaseTests).GetMethod(
                "DummyTestMethod",
                BindingFlags.Instance | BindingFlags.NonPublic));

        private void DummyTestMethod() { }
    }
}
