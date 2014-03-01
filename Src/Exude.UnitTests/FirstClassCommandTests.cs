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
    public class FirstClassCommandTests
    {
        [Fact]
        public void SutIsTestCommand()
        {
            Action<object> dummyAction = _ => { };
            var sut = new FirstClassCommand(dummyAction, dummyMethod);
            Assert.IsAssignableFrom<ITestCommand>(sut);
        }

        [Fact]
        public void TestActionIsCorrect()
        {
            Action<object> expected = _ => { };
            var sut = new FirstClassCommand(expected, dummyMethod);

            Action<object> actual = sut.TestAction;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ExecuteInvokesAction()
        {
            var verified = false;
            var obj = new object();
            Action<object> spy = x => verified = x == obj;
            var sut = new FirstClassCommand(spy, dummyMethod);

            sut.Execute(obj);

            Assert.True(verified, "Spy should have been invoked.");
        }

        [Fact]
        public void ExecuteSuccessfullyReturnsCorrectResult()
        {
            Action<object> testAction = _ => { };
            var sut = new FirstClassCommand(testAction, anotherMethod);

            var actual = sut.Execute(new object());

            var pr = Assert.IsAssignableFrom<PassedResult>(actual);
            Assert.Equal(anotherMethod.Name, pr.MethodName);
            Assert.Equal(anotherMethod.TypeName, pr.TypeName);
        }

        [Fact]
        public void ConstructorCorrectlyAssignsTestMethod()
        {
            Action<object> testAction = _ => { };
            var expected = anotherMethod;
            var sut = new FirstClassCommandInspector(testAction, expected);
            Assert.IsAssignableFrom<FirstClassCommand>(sut);

            var actual = sut.TestMethodInspectionValue;

            Assert.Equal(expected, actual);
        }

        private class FirstClassCommandInspector : FirstClassCommand
        {
            public FirstClassCommandInspector(
                Action<object> testAction,
                IMethodInfo testMethod)
                : base(testAction, testMethod)
            {
            }

            public IMethodInfo TestMethodInspectionValue
            {
                get { return this.testMethod; }
            }
        }

        [Fact]
        public void ConstructWithNullTestActionThrows()
        {
            Assert.Throws<ArgumentNullException>(
                () => new FirstClassCommand(null, dummyMethod));
        }

        [Fact]
        public void TimeoutIsCorrect()
        {
            var sut = new FirstClassCommand(_ => { }, dummyMethod);

            var actual = sut.Timeout;

            var expected =
                MethodUtility.GetTimeoutParameter(
                    Reflector.Wrap(sut.TestAction.Method));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayNameIsNotEmpty()
        {
            var sut = new FirstClassCommand(_ => { }, dummyMethod);
            var actual = sut.DisplayName;
            Assert.False(
                string.IsNullOrEmpty(actual),
                "DisplayName should not be null or empty.");
        }

        private readonly static IMethodInfo dummyMethod =
            Reflector.Wrap(typeof(FirstClassCommandTests).GetMethod(
                "DummyTestMethod",
                BindingFlags.Instance | BindingFlags.NonPublic));

        private void DummyTestMethod() { }

        private readonly static IMethodInfo anotherMethod =
            Reflector.Wrap(typeof(FirstClassCommandTests).GetMethod(
                "AnotherTestMethod",
                BindingFlags.Instance | BindingFlags.NonPublic));

        private void AnotherTestMethod() { }
    }
}
